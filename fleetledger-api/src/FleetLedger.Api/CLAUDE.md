# CLAUDE.md — FleetLedger.Api

Complementa `fleetledger-api/CLAUDE.md`.

## Estrutura

```
Program.cs                 lista de intenções: só chama Add*/Use*
Common/Api/                IEndpoint, MapEndpoint, ValidationFilter, AddValidators,
                           CORS, contrato HTTP (OpenAPI, JSON, ProblemDetails)
Common/Health/             health checks registrados e contrato da resposta
Common/Observability/      Serilog + OpenTelemetry
Common/Security/           Identity (política de bloqueio), JWT (validação e emissão),
                           verificação de credenciais, GetUserId, UserContext
Common/Time/               IBusinessCalendar: "hoje" no fuso de Calendar:TimeZone
Data/                      AppDbContext, convenções de modelo, ForOrganization,
                           Mappings/, Migrations/ (geradas — não editar à mão),
                           Seed/ (administrador, organização e Plano de Contas fixo),
                           Stores/<Modulo>/ (acesso a dados de cada handler)
Endpoints/                 EndpointMapper + <Modulo>/ (um arquivo por rota; Health/ é o molde)
Handlers/<Modulo>/         regra de negócio (Auth/, LedgerAccounts/…)
Models/                    SOMENTE entidades acopladas à infra (User : IdentityUser)
Options/                   Options + validador de cada seção de configuração
```

## Endpoints

```csharp
public sealed class CreateVehicleEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Vehicles: Create")
            .WithSummary("Cadastra um veículo")
            .AddEndpointFilter<ValidationFilter<CreateVehicleRequest>>()
            .Produces<Response<Vehicle?>>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

    private static async Task<IResult> HandleAsync(
        IUserContext userContext,
        IVehicleHandler handler,
        CreateVehicleRequest request,
        CancellationToken cancellationToken)
    {
        request.OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken);

        Response<Vehicle?> result = await handler.CreateAsync(request, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created($"/v1/vehicles/{result.Data?.Id}", result)
            : TypedResults.Json(result, statusCode: result.Code);
    }
}
```

- Zero regra de negócio no endpoint: traduz HTTP, atribui `OrganizationId`, delega.
- Grupo por módulo (`MapGroup("v1/vehicles").WithTags(...)`), registrado no `EndpointMapper`.
- A autenticação já é exigida pela `FallbackPolicy`; não repita `RequireAuthorization()`.

## Handlers

- Implementam a interface de `Core`; injeção por construtor primário.
- Dados só pelo `I<Entidade>Store` do módulo (`Data/Stores/`), nunca pelo `AppDbContext`:
  é o que deixa o handler testável sem banco (docs/adr/0004).
- No store, consulta de `IOrganizationOwned` sempre via `ForOrganization`; leitura com `AsNoTracking()`.
- Dado global, sem organização (Plano de Contas), é exceção explícita e comentada.
- Datas via `TimeProvider` injetado; data de negócio ("hoje", vencimentos) via `IBusinessCalendar`,
  nunca `DateOnly.FromDateTime(DateTime.Now)`. Mensagens de usuário numa fábrica `<Modulo>Responses`.
- Acima de 200 linhas, divida por operação.

## Data

- `DataExtension.UsePostgres` é o único lugar que nomeia o banco.
- Mapeamento por entidade em `Data/Mappings/`, aplicado por `ApplyConfigurationsFromAssembly`.
  Não repita chave, datas, FK ou índice de organização: as convenções de
  `ModelBuilderExtension` já aplicam.
- Prefira `HasMaxLength`/`HasPrecision` a `HasColumnType`.
- Migration com nome descritivo; `Down` sempre implementado. Confira os nomes de
  tabela, FK e índice gerados antes de aceitar.

## Configuração

- Seção nova = `<Assunto>Options` + `<Assunto>OptionsValidator` + uma linha em
  `OptionsRegistrationExtension`.
- `appsettings.json` traz só a forma (segredos vazios).
