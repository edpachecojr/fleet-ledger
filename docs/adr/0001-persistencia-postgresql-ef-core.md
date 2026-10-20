# ADR 0001 — Persistência: PostgreSQL + EF Core

**Data:** 2026-10-20 · **Status:** aceito

## Contexto
O sistema é transacional (viagens, títulos, faturas), com relatórios agregados (DRE, rateio) e precisa de integridade referencial e transações.

## Decisão
- **PostgreSQL** como banco, acessado por **EF Core** (`Npgsql.EntityFrameworkCore.PostgreSQL`) com migrations em `fleetledger-api/src/FleetLedger.Api/Data/Migrations`.
- Nomes no banco em `snake_case` via `EFCore.NamingConventions`: tabelas no plural, entidades no singular. Tabelas do Identity sem prefixo `AspNet` (`users`, `user_claims`…).
- O provider é nomeado num único ponto: `DataExtension.UsePostgres`. Os testes usam o mesmo método.
- `Id` é UUID v7 gerado na aplicação (`Guid.CreateVersion7()`), nunca pelo banco. Datas são `DateTimeOffset` em UTC (`timestamptz`, precisão de microssegundos).
- Configuração comum de `BaseModel` (chave, precisão das datas) aplicada uma vez em `ModelBuilderExtension`, não repetida por entidade.
- Migrations aplicadas na inicialização apenas quando `Database:ApplyMigrationsOnStartup=true` (desenvolvimento e docker compose). Em produção, etapa de deploy.

## Alternativas consideradas
- **SQL Server**: licenciamento e custo de hospedagem maiores, sem ganho funcional para o caso.
- **Dapper/SQL puro**: mais controle, porém sem migrations versionadas nem mapeamento do Identity prontos.

## Consequências
- Tipos específicos do PostgreSQL (`timestamptz`, `uuid`) ficam confinados a `Api/Data`.
- `Core` continua sem nenhuma dependência de persistência.
