# CLAUDE.md — tests/

Complementa `fleetledger-api/CLAUDE.md`. Decisão em `docs/adr/0004`.

## Stack

xUnit 2 · FluentAssertions 7 (não atualize para 8: licença comercial) ·
NSubstitute · coverlet. Pacotes e `using` globais já vêm de `Directory.Build.props`.

## Escopo

**Só testes unitários.** Nenhum teste abre conexão com banco, rede, disco ou
relógio real. Integração ponta a ponta virá num projeto próprio, com ADR.

## Estrutura

`tests/` espelha `src/`:

```
src/FleetLedger.Api/Common/Security/UserContext.cs
tests/FleetLedger.Api.Tests/Common/Security/UserContextTests.cs
```

```
FleetLedger.Api.Tests/
├── Fakes/      FakeUserOrganizationReader, FakeValidator, SampleRequest
├── Fixtures/   OfflineDbContextFactory, HybridCacheFactory, HttpContextAccessorFactory
└── <espelho de src>/
```

## Regras

- Nome: `Metodo_Cenario_ResultadoEsperado`.
- Arrange / Act / Assert separados por linha em branco, sem comentário marcando.
- Uma asserção conceitual por teste.
- I/O vira **fake nomeado** em `Fakes/`; NSubstitute só para verificar interação.
- Modelo EF e SQL gerado são verificados com `OfflineDbContextFactory`
  (`IDesignTimeModel`, `ToQueryString()`), que nunca conecta.
- Correção de bug: o teste falha antes da correção.
- Todo módulo: validador (válido + um caso por regra), handler (sucesso, não
  encontrado) e **isolamento entre organizações**.

## Comando

```bash
dotnet test                                   # tudo
dotnet test --collect:"XPlat Code Coverage"   # cobertura (piso: 70% em Core e Api)
```
