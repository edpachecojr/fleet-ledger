# ADR 0004 — Testes: unitários com fakes, sem banco

**Data:** 2026-10-20 · **Status:** aceito

## Contexto
O guia (`docs/engenharia-agentes/04-testes.md`) sugere SQLite in-memory, que diverge do PostgreSQL (`snake_case`, `timestamptz`, `uuid`). A equipe prefere, neste momento, só testes unitários.

## Decisão
- `dotnet test` roda apenas testes unitários: sem banco, sem rede, sem Docker.
- I/O é substituído por **fakes nomeados** em `tests/*/Fakes/` (ex.: `FakeUserOrganizationReader`). Handlers que consultam dados dependem de uma abstração pequena e substituível.
- Modelo EF e SQL gerado são verificados **sem conexão** (`OfflineDbContextFactory`: `ToQueryString()` e o `IDesignTimeModel`).
- Stack: xUnit 2 · FluentAssertions **7.x** (a 8.x exige licença comercial) · NSubstitute · coverlet.
- Testes de integração ponta a ponta (PostgreSQL real, provavelmente via Testcontainers) ficam para outro momento, num projeto `FleetLedger.Api.IntegrationTests`, com ADR próprio.

## Consequências
- Comportamentos que só o banco tem (constraint única, concorrência) não são cobertos até existirem os testes de integração.
