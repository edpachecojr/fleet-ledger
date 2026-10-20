# ADR 0005 — Contrato HTTP: Minimal API + OpenAPI + Scalar

**Data:** 2026-10-20 · **Status:** aceito

## Decisão
- **Minimal APIs** com um arquivo por rota (`IEndpoint`) e um `<Modulo>EndpointGroup` por módulo. Sem controllers.
- Documento **OpenAPI** (`Microsoft.AspNetCore.OpenApi`) publicado em todo ambiente em `/openapi/v1.json`; UI **Scalar** (`/scalar`) apenas em desenvolvimento.
- JSON `camelCase`, enums como texto, `ProblemDetails` com `traceId`, envelope `Response<T>` com `Code` serializado.

## Alternativas consideradas
- **Controllers**: mais cerimônia e acoplamento a convenções do MVC sem benefício aqui.
- **Swashbuckle**: o gerador nativo do .NET 10 cobre o necessário sem dependência extra de terceiros.
