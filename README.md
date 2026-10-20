# FleetLedger — Gestão de Transportadoras (MVP)

Monorepo do MVP. Hoje contém o back-end (`fleetledger-api/`); o front-end virá em pasta própria.

- Backlog, roadmap e progresso: [`docs/`](docs/)
- Decisões de arquitetura: [`docs/adr/`](docs/adr/)
- Regras para agentes e pessoas: [`CLAUDE.md`](CLAUDE.md) e [`fleetledger-api/CLAUDE.md`](fleetledger-api/CLAUDE.md)

## Pré-requisitos

- Docker com Compose
- .NET SDK 10 (só para desenvolver/testar fora do container)

## Subir tudo (banco + API)

```bash
cp .env.example .env        # ajuste a senha e a JWT_SIGNING_KEY
docker compose up --build
```

| O quê | Onde |
| --- | --- |
| Saúde (inclui conexão com o banco) | http://localhost:5080/health |
| Documento OpenAPI | http://localhost:5080/openapi/v1.json |
| UI da API (Scalar, só em Development) | http://localhost:5080/scalar |
| PostgreSQL | `localhost:5433` (usuário/banco `fleetledger`) |

As migrations são aplicadas automaticamente na subida. `/health` responde `200` com o banco acessível e `503` sem ele.

## Rodar os testes

```bash
cd fleetledger-api
dotnet test
```

Só testes unitários: não precisam de banco nem de Docker.

## Rodar a API fora do container

Com o PostgreSQL do compose no ar (`docker compose up -d postgres`):

```bash
cd fleetledger-api
dotnet user-secrets --project src/FleetLedger.Api set "Database:ConnectionString" "Host=localhost;Port=5433;Database=fleetledger;Username=fleetledger;Password=<POSTGRES_PASSWORD do .env>"
dotnet user-secrets --project src/FleetLedger.Api set "Jwt:SigningKey" "<chave com 32 bytes ou mais>"
dotnet run --project src/FleetLedger.Api       # http://localhost:5080
```

Sem essas chaves a API não sobe e informa qual configuração falta.

## Configuração

| Chave | Descrição |
| --- | --- |
| `Database:ConnectionString` | conexão PostgreSQL (segredo) |
| `Database:ApplyMigrationsOnStartup` | aplica migrations na subida (`true` em Development) |
| `Jwt:Issuer` / `Jwt:Audience` | emissor e público do access token |
| `Jwt:SigningKey` | chave HS256, mínimo de 32 bytes (segredo) |
| `Cors:AllowedOrigins` | origens de navegador permitidas (vazio por padrão) |
| `Otlp:Endpoint` | coletor OpenTelemetry; vazio desliga a exportação |
