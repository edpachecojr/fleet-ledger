# FleetLedger

**Gestão operacional e financeira para transportadoras.** Frota, viagens, acerto de motoristas, contas a pagar e a receber, manutenção preventiva e resultado (DRE) por viagem e por caminhão, num backoffice web sobre uma API .NET.

[![Licença: MIT](https://img.shields.io/badge/licen%C3%A7a-MIT-blue.svg)](LICENSE)
![.NET 10](https://img.shields.io/badge/.NET-10-512BD4)
![React 19](https://img.shields.io/badge/React-19-61DAFB)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-17-4169E1)

> **Status:** em desenvolvimento (MVP). Fundação concluída: API, backoffice e banco sobem juntos; os módulos de negócio estão em construção. Acompanhe em [`docs/PROGRESS.md`](docs/PROGRESS.md).

---

## Sumário

- [O problema](#o-problema)
- [Funcionalidades do MVP](#funcionalidades-do-mvp)
- [Arquitetura](#arquitetura)
- [Stack](#stack)
- [Estrutura do repositório](#estrutura-do-repositório)
- [Como rodar](#como-rodar)
- [Desenvolvimento](#desenvolvimento)
- [Testes e qualidade](#testes-e-qualidade)
- [Configuração](#configuração)
- [Documentação](#documentação)
- [Convenções](#convenções)
- [Licença](#licença)

## O problema

Pequenas e médias transportadoras costumam controlar viagens, adiantamentos, despesas e manutenção em planilhas separadas. Sem cruzar esses dados, o gestor não sabe quanto cada viagem e cada caminhão realmente deram de lucro. O FleetLedger junta a operação e o financeiro num só lugar, e é dessa base que sai o resultado econômico da frota.

## Funcionalidades do MVP

| Marco | O que entrega | Status |
| --- | --- | --- |
| M0 · Fundação | Aplicação executável e acesso restrito ao administrador | 🟨 em andamento |
| M1 · Cadastros | Plano de contas, veículos (com status operacional), clientes e motoristas | ⬜ |
| M2 · Financeiro base | Contas a pagar, baixa de títulos e despesas fixas da frota | ⬜ |
| M3 · Viagens | Abertura, adiantamentos, despesas, remuneração do motorista, fechamento e acerto | ⬜ |
| M4 · Faturamento | Fila de faturamento, faturas, contas a receber, cancelamento e extrato de caixa | ⬜ |
| M5 · Manutenção | Planos por veículo, semáforo de alertas e registro de manutenção executada | ⬜ |
| M6 · Resultados | Rateio de custo fixo, DRE por viagem, DRE mensal por veículo e dashboards | ⬜ |

Detalhes de cada marco em [`docs/ROADMAP.md`](docs/ROADMAP.md); as 32 tarefas, com critérios de aceite, em [`docs/backlog/`](docs/backlog/README.md).

## Arquitetura

```
 Navegador ──► backoffice (nginx) ──/api/*──► API .NET ──► PostgreSQL
               SPA React estática             Minimal API
                                              OpenAPI /openapi/v1.json
```

- **API agnóstica de UI.** O contrato é o documento OpenAPI. O backoffice gera os tipos TypeScript a partir dele (`pnpm api:types`) e não escreve tipo de API à mão.
- **Mesma origem.** O navegador sempre chama `/api`: o nginx (no compose) ou o Vite (em desenvolvimento) encaminham para a API. Não há CORS a configurar.
- **Isolamento por organização.** Todo dado de uma empresa carrega `OrganizationId`, e toda consulta o filtra de forma explícita (`ForOrganization`). A organização vem do usuário autenticado, nunca da requisição. Ver [ADR 0003](docs/adr/0003-isolamento-por-organizacao.md).
- **Back-end em camadas.** `FleetLedger.Core` guarda só contratos (entidades, requisições, validadores, interfaces) e não depende de nenhum pacote; `FleetLedger.Api` implementa HTTP, persistência e segurança.

## Stack

| Camada | Tecnologia |
| --- | --- |
| API | .NET 10, ASP.NET Core Minimal API, OpenAPI + Scalar |
| Persistência | PostgreSQL 17, EF Core 10 (migrations, `snake_case`), IDs UUID v7 |
| Autenticação | ASP.NET Core Identity + JWT bearer |
| Observabilidade | Serilog + OpenTelemetry (exportação OTLP opcional) |
| Backoffice | React 19, TypeScript 6, Vite 8, React Router 8, TanStack Query 5 |
| UI | Tailwind CSS 4, design system Backoffice, ícones lucide |
| Testes | xUnit + FluentAssertions + NSubstitute (API) · Vitest + Testing Library (web) |
| Infraestrutura local | Docker Compose, nginx |

Cada escolha tem motivo e alternativas registrados em [`docs/adr/`](docs/adr/).

## Estrutura do repositório

```
.
├── fleetledger-api/              API .NET 10
│   ├── src/FleetLedger.Core/     contratos: entidades, requisições, validadores
│   ├── src/FleetLedger.Api/      endpoints, persistência, segurança, observabilidade
│   └── tests/                    testes unitários de Core e Api
├── fleetledger-backoffice-web/   backoffice React (SPA)
│   ├── src/api/                  cliente HTTP tipado pelo OpenAPI
│   ├── src/design-system/        tokens, tema e componentes genéricos
│   ├── src/layout/               shell: sidebar, header, navegação
│   └── src/features/             uma pasta por funcionalidade
├── docs/
│   ├── backlog/                  tarefas do MVP (TASK-001 … TASK-032)
│   ├── adr/                      decisões de arquitetura
│   ├── ROADMAP.md                marcos
│   └── PROGRESS.md               andamento, decisões e diário
├── docker-compose.yml            banco + API + backoffice
└── .env.example                  variáveis do compose
```

## Como rodar

**Pré-requisito:** Docker com Compose.

```bash
git clone https://github.com/edpachecojr/fleet-ledger.git
cd fleet-ledger
cp .env.example .env      # troque a senha e gere uma JWT_SIGNING_KEY (openssl rand -base64 48)
docker compose up --build
```

| Serviço | Endereço |
| --- | --- |
| Backoffice | http://localhost:3000 |
| API: saúde (inclui o banco) | http://localhost:5080/health |
| API: documento OpenAPI | http://localhost:5080/openapi/v1.json |
| API: referência interativa (Scalar) | http://localhost:5080/scalar |
| PostgreSQL | `localhost:5433` (usuário e banco `fleetledger`) |

As migrations são aplicadas na subida. `/health` responde `200` com o banco acessível e `503` sem ele, e o backoffice mostra esse estado na página inicial.

## Desenvolvimento

**Pré-requisitos:** .NET SDK 10, Node 24 e pnpm 11.

### API

Com o banco no ar (`docker compose up -d postgres`), configure os segredos locais uma vez:

```bash
cd fleetledger-api
dotnet user-secrets --project src/FleetLedger.Api set "Database:ConnectionString" "Host=localhost;Port=5433;Database=fleetledger;Username=fleetledger;Password=<POSTGRES_PASSWORD do .env>"
dotnet user-secrets --project src/FleetLedger.Api set "Jwt:SigningKey" "<chave com 32 bytes ou mais>"
dotnet run --project src/FleetLedger.Api        # http://localhost:5080
```

Sem essas chaves a API não sobe e diz qual configuração falta. Nova migration:

```bash
dotnet ef migrations add <NomeDescritivo> --project src/FleetLedger.Api --output-dir Data/Migrations
```

### Backoffice

Com a API no ar (local ou `docker compose up -d postgres api`):

```bash
cd fleetledger-backoffice-web
pnpm install
pnpm dev          # http://localhost:5173 — /api é encaminhado para http://localhost:5080
pnpm api:types    # após mudar a API: regenera src/api/generated/schema.d.ts
```

## Testes e qualidade

Testes unitários, rápidos e sem dependências externas: não precisam de banco, API no ar nem Docker.

```bash
# API
cd fleetledger-api
dotnet test
dotnet format --verify-no-changes

# Backoffice
cd fleetledger-backoffice-web
pnpm test
pnpm typecheck && pnpm lint && pnpm format:check && pnpm build
```

Warnings de compilação são erros nos dois projetos. O ESLint do backoffice também barra funções com mais de 30 linhas, arquivos com mais de 300 e aninhamento além de dois níveis.

## Configuração

**Compose (`.env`, não versionado):**

| Variável | Descrição |
| --- | --- |
| `POSTGRES_PASSWORD` | senha do banco local |
| `JWT_SIGNING_KEY` | chave HS256 do access token, mínimo de 32 bytes |
| `POSTGRES_PORT` / `API_PORT` / `WEB_PORT` | portas no host (padrão 5433 / 5080 / 3000) |

**API (`appsettings.json`, user-secrets ou variáveis de ambiente com `__`):**

| Chave | Descrição |
| --- | --- |
| `Database:ConnectionString` | conexão PostgreSQL (segredo) |
| `Database:ApplyMigrationsOnStartup` | aplica migrations na subida (`true` em Development) |
| `Jwt:Issuer` / `Jwt:Audience` / `Jwt:SigningKey` | validação do access token |
| `Cors:AllowedOrigins` | origens de navegador permitidas (vazio: não há chamada cross-origin) |
| `Otlp:Endpoint` | coletor OpenTelemetry; vazio desliga a exportação |

Configuração inválida derruba a API na subida com mensagem que diz a chave e o valor esperado. Segredos nunca vão para arquivos versionados.

## Documentação

- [Roadmap](docs/ROADMAP.md) e [progresso](docs/PROGRESS.md)
- [Backlog](docs/backlog/README.md), com critérios de aceite por tarefa
- Decisões de arquitetura:
  [0001 Persistência](docs/adr/0001-persistencia-postgresql-ef-core.md) ·
  [0002 Autenticação](docs/adr/0002-autenticacao-identity-jwt.md) ·
  [0003 Isolamento por organização](docs/adr/0003-isolamento-por-organizacao.md) ·
  [0004 Testes](docs/adr/0004-testes-unitarios-sem-banco.md) ·
  [0005 Contrato HTTP](docs/adr/0005-contrato-http-minimal-api-openapi.md) ·
  [0006 Front-end](docs/adr/0006-frontend-backoffice-vite-spa.md)

## Convenções

As regras de código valem para pessoas e agentes de IA e ficam em [`CLAUDE.md`](CLAUDE.md) (geral), [`fleetledger-api/CLAUDE.md`](fleetledger-api/CLAUDE.md) e [`fleetledger-backoffice-web/CLAUDE.md`](fleetledger-backoffice-web/CLAUDE.md). Em resumo: funções pequenas com uma responsabilidade, nomes que revelam intenção, tipos explícitos, dependências injetadas, erros com contexto e teste junto com o código.

Commits em português, no imperativo, com prefixo (`feat:`, `fix:`, `refactor:`, `test:`, `docs:`, `chore:`).

## Licença

Distribuído sob a licença MIT. Veja [`LICENSE`](LICENSE).
