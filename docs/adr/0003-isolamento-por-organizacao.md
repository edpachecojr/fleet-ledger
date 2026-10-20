# ADR 0003 — Isolamento lógico por organização

**Data:** 2026-10-20 · **Status:** aceito

## Contexto
Os dados pertencem a uma empresa (organização). O isolamento precisa ser visível no código e verificável em teste, sem depender de comportamento implícito.

## Decisão
- `Organization` é entidade própria; `User` pertence a uma organização.
- Toda entidade de uma organização implementa `IOrganizationOwned` (`OrganizationId`). FK para `organizations` e índice em `organization_id` são aplicados a todas por `ModelBuilderExtension.ApplyOrganizationOwnership`; índice único composto fica no mapeamento da entidade quando a cardinalidade exigir.
- `IUserContext` expõe `UserId` (claims do access token) e `GetOrganizationIdAsync`, que busca a organização no banco a partir do usuário, com `HybridCache` de 5 minutos. Usuário inexistente lança `UserNotFoundException` com o id.
- **Leitura:** toda consulta de `IOrganizationOwned` passa por `DbSet<T>.ForOrganization(organizationId)`. Única exceção: `UserOrganizationReader`, que é quem descobre a organização.
- **Escrita:** o endpoint atribui `OrganizationId` explicitamente, a partir de `IUserContext`. `Request.OrganizationId` é `[JsonIgnore]`: não entra no contrato nem pode ser enviado pelo cliente.
- A organização **nunca** vem de header, query string ou body.

## Proibido
Query filters globais (`HasQueryFilter`), interceptors, override de `SaveChanges` para preencher a organização. A palavra "tenant" não aparece no código.

## Consequências
- Esquecer o `ForOrganization` é possível; por isso todo módulo tem teste de isolamento entre organizações.
- O cache de 5 minutos significa que mudar a organização de um usuário leva até 5 minutos para valer.
