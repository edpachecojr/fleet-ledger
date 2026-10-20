# CLAUDE.md — fleetledger-api

Contrato de desenvolvimento do back-end. Complementa o `CLAUDE.md` da raiz, que
continua valendo integralmente. Decisões de infraestrutura e seus motivos em
`docs/adr/` — consulte antes de decidir qualquer padrão.

## O projeto

API HTTP do MVP de gestão de transportadoras. Agnóstica de UI: clientes consomem
o contrato OpenAPI, nunca um assembly desta solução.

**Módulos planejados** (ver `docs/ROADMAP.md`): Plano de Contas, Veículos,
Clientes, Motoristas, Contas a Pagar, Viagens, Faturamento, Contas a Receber,
Manutenção, Resultados (DRE).

| Camada | Escolha | ADR |
| --- | --- | --- |
| Plataforma | .NET 10 · C# latest | — |
| API | ASP.NET Core Minimal API (`IEndpoint`), sem controllers | `docs/adr/0005` |
| Persistência | EF Core + PostgreSQL, `snake_case` | `docs/adr/0001` |
| Autenticação | ASP.NET Core Identity + JWT bearer | `docs/adr/0002` |
| Isolamento | por organização, explícito na aplicação | `docs/adr/0003` |
| Documentação da API | OpenAPI nativo + Scalar (dev) | `docs/adr/0005` |
| Cache | `HybridCache` (em memória) | `docs/adr/0003` |
| Worker | não há | — |
| Observabilidade | Serilog (console) + OpenTelemetry (OTLP opcional) | fixo |
| Testes | xUnit + FluentAssertions 7 + NSubstitute, só unitários | `docs/adr/0004` |

Escolha que não está nesta tabela e num ADR não foi feita — pergunte.

## Estrutura

```
fleetledger-api/
├── FleetLedger.slnx
├── Directory.Build.props      configuração comum (warning = erro, nullable)
├── Directory.Packages.props   TODAS as versões de pacote
├── src/
│   ├── FleetLedger.Core/             contratos. Zero pacote externo.
│   └── FleetLedger.Api/              API, persistência, segurança. Referencia Core.
└── tests/
    ├── FleetLedger.Core.Tests/
    └── FleetLedger.Api.Tests/
```

| Artefato | Caminho |
| --- | --- |
| Entidade de domínio | `Core/Models/<Entidade>.cs` (herda `BaseModel`) |
| Requisição | `Core/Requests/<Modulo>/<Verbo><Entidade>Request.cs` |
| Validador | `Core/Validators/<Modulo>/<Verbo><Entidade>Validator.cs` |
| Contrato de handler | `Core/Handlers/I<Modulo>Handler.cs` |
| Regra de negócio | `Api/Handlers/<Modulo>/<Modulo>Handler.cs` |
| Rota HTTP | `Api/Endpoints/<Modulo>/<Verbo><Entidade>Endpoint.cs` |
| Registro do grupo | `Api/Endpoints/<Modulo>/<Modulo>EndpointGroup.cs` |
| Máquina de estados | `Api/Handlers/<Modulo>/<Entidade>StatusTransition.cs` |
| Mapeamento EF | `Api/Data/Mappings/<Entidade>Mapping.cs` |
| Configuração | `Api/Options/<Assunto>Options.cs` + `<Assunto>OptionsValidator.cs` |
| Teste | `tests/FleetLedger.<Camada>.Tests/<mesmo caminho de origem>Tests.cs` |

Arquivo fora do lugar é defeito, mesmo que compile.

## Comandos

```bash
dotnet build FleetLedger.slnx                 # zero warning (warning é erro)
dotnet test                            # toda a suíte, sem banco nem preparo
dotnet format                          # formatar
dotnet format --verify-no-changes      # verificar formatação

# Migrations (precisa de Database:ConnectionString e Jwt:SigningKey configurados)
dotnet ef migrations add <NomeDescritivo> --project src/FleetLedger.Api --output-dir Data/Migrations
dotnet ef database update --project src/FleetLedger.Api
```

Subir tudo localmente: `docker compose up --build` na raiz (ver `README.md`).

## Modelo base

- **Toda entidade herda `BaseModel`**: `Id` (UUID v7 gerado pela aplicação,
  nunca pelo banco), `CreatedAt`/`UpdatedAt` (`DateTimeOffset` UTC), igualdade
  e `GetHashCode` por tipo + `Id`.
- `CreatedAt`/`UpdatedAt` são atribuídos **explicitamente** por quem cria ou
  altera, com `TimeProvider` injetado. Sem interceptor, sem override de
  `SaveChanges`.
- Configuração comum (chave, `ValueGeneratedNever`, precisão das datas) vive
  **só** em `Data/ModelBuilderExtension.ApplyBaseModelConfiguration`. O
  mapeamento da entidade não repete nada disso.
- Banco em `snake_case`; tabela no plural (`ToTable("vehicles")`), entidade no
  singular (`Vehicle`).

## Isolamento por organização 🔴

Isolamento lógico por coluna, explícito, aplicado pela aplicação. A palavra
"tenant" não aparece no código.

- Entidade de uma organização implementa `IOrganizationOwned`. FK e índice em
  `organization_id` são automáticos; **índice único composto** com
  `OrganizationId` fica no mapeamento quando a cardinalidade exigir (ex.: placa
  única por organização).
- **Leitura:** toda consulta de `IOrganizationOwned` começa por
  `context.<DbSet>.ForOrganization(organizationId)`.
- **Escrita:** o endpoint atribui `request.OrganizationId = await
  userContext.GetOrganizationIdAsync(cancellationToken)`; o handler copia para a
  entidade.
- A organização vem **só** de `IUserContext`. Nunca de header, query string ou
  body (`Request.OrganizationId` é `[JsonIgnore]`).
- **Proibido:** `HasQueryFilter`, filtro global, interceptor, override de
  `SaveChanges` para preencher a organização.
- Todo módulo tem teste provando que uma organização não lê nem altera dado de
  outra.

## Validação

- `DataAnnotations` é proibido. Um validador por Request em
  `Core/Validators/<Modulo>/`, herdando `Validator<TRequest>`, registrado
  automaticamente por `AddValidators()`.
- Todo endpoint que recebe corpo tem `.AddEndpointFilter<ValidationFilter<TRequest>>()`.
- Configuração: `IValidateOptions<T>` + `ValidateOnStart()`, reaproveitando o
  `ValidationBuilder` de Core. Mensagem traz a chave completa e o valor recebido
  (nunca o valor de um segredo).
- Todo validador tem teste: um caso válido e um por regra.

## Tratamento de erro

- Nenhum `catch` sem tipo específico; nenhum `catch` sem
  `logger.LogError(exception, ...)`.
- Erro de negócio é `Response<T>` com código HTTP, não exceção. O endpoint
  devolve `TypedResults.Json(result, statusCode: result.Code)`.
- Exceção inesperada sobe até `UseExceptionHandler`, que responde
  `ProblemDetails` com `traceId`.
- Mensagem de exceção: o que se tentou, com qual valor, o que se esperava.
  `throw;`, nunca `throw ex;`.

## Observabilidade

- Serilog + OpenTelemetry, stack única. Console legível em Development, JSON
  compacto fora dele. OTLP liga quando `Otlp:Endpoint` está preenchido.
- Message template com propriedades nomeadas; **nunca** interpolação em log.
- Evento de negócio relevante → `LogInformation` com ids e resultado. Nunca
  segredo, senha, token ou PII.
- Métrica sem tag de alta cardinalidade (`UserId`, `OrganizationId`).

## Segurança

- `UserId` vem de `ClaimTypes.NameIdentifier` (`sub`) via `GetUserId()`, que
  lança em vez de devolver `Guid.Empty`.
- Autorização fechada por padrão (`FallbackPolicy`). Rota anônima declara
  `AllowAnonymous()` com motivo.
- Segredo nunca em `appsettings*.json`: `dotnet user-secrets` em dev, `.env`
  (não versionado) no compose.
- CORS só para `Cors:AllowedOrigins`. Nunca `AllowAnyOrigin`.

## Contrato HTTP

- Rotas `v1/<recurso>` (plural, minúsculo). Todo endpoint com `WithName`,
  `WithSummary`, `Produces<T>` por status e `ProducesValidationProblem` quando
  valida.
- JSON `camelCase`, datas ISO 8601 com offset, enum como texto, envelope
  `Response<T>`.
- Mudança que quebra contrato vai para `/v2/...`.

## Fluxo de uma funcionalidade

1. `Core/Models/` — entidade (`BaseModel`, `IOrganizationOwned` se for o caso)
2. `Core/Requests/<Modulo>/` — POCO, sem atributo
3. `Core/Validators/<Modulo>/` — o validador
4. `Core/Handlers/I<Modulo>Handler.cs` — assinatura com `CancellationToken`
5. `Api/Data/Mappings/` + `dotnet ef migrations add <Nome>`
6. `Api/Handlers/<Modulo>/` — regra, `ForOrganization`, log, `catch` tipado
7. `Api/Endpoints/<Modulo>/` — rota com `ValidationFilter` + registro no grupo
8. `tests/` — validador, handler (sucesso, não encontrado, isolamento entre organizações)

**Antes de declarar concluído:** `dotnet build` sem warning · `dotnet test`
verde · `dotnet format --verify-no-changes` limpo · nenhum `catch` sem tipo ou
sem log, `#region`, `async void`, `DateTime.Now` ou interpolação em log ·
nenhum arquivo acima de 300 linhas · este arquivo ainda verdadeiro ·
`docs/PROGRESS.md` atualizado.
