# CLAUDE.md — FleetLedger.Core

Complementa `fleetledger-api/CLAUDE.md`.

## Responsabilidade

Contratos internos do back-end: entidades, requisições, respostas, validadores e
interfaces de handler. Nada mais.

## Regra de dependência 🔴

**Nenhum `PackageReference` aqui.** Sem EF Core, sem ASP.NET, sem SDK. Se parece
necessário, o código pertence a `FleetLedger.Api`. Atributos da BCL
(`System.Text.Json.Serialization`) são permitidos.

## Estrutura

```
Common/Constants/    const de verdade (PagingDefaults)
Common/Validation/   IValidator, Validator, ValidationBuilder, ValidationResult, ValidationError
Models/              BaseModel, IOrganizationOwned, Organization, entidades
Requests/            Request, PagedRequest, <Modulo>/
Responses/           Response<T>, PagedResponse<T>
Security/            IUserContext
Handlers/            SOMENTE interfaces I<Modulo>Handler
Validators/<Modulo>/ um validador por Request
```

## Regras

- Entidade é POCO: sem atributo de persistência; mapeamento em `Api/Data/Mappings/`.
- Entidade herda `BaseModel`; se pertence a uma organização, implementa `IOrganizationOwned`.
- Request herda `Request` (ou `PagedRequest`) e não tem atributo de validação.
  Exceção: `LoginRequest`, anônima, não tem organização a atribuir.
- `Response<T>.Code` é público e serializado (antipadrão A-02, travado por teste).
- Enum com prefixo `E` e valor explícito (`Active = 1`): o número é persistido.
- Todo membro público tem XML doc com intenção e, quando não for óbvio, exemplo
  (CS1591 é erro neste projeto).
