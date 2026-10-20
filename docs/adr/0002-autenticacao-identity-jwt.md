# ADR 0002 — Autenticação: ASP.NET Core Identity + JWT

**Data:** 2026-10-20 · **Status:** aceito

## Contexto
A API é agnóstica de UI: qualquer cliente (web, mobile, outro serviço) a consome. O MVP tem um único perfil, o administrador (TASK-002).

## Decisão
- **ASP.NET Core Identity** (`AddIdentityCore` + `IdentityUserContext<User, Guid>`, sem roles) para usuário e hash de senha.
- **JWT bearer** como access token, validado com `Jwt:Issuer`, `Jwt:Audience` e `Jwt:SigningKey` (HS256, mínimo 32 bytes), com `ValidateOnStart`.
- O id do usuário vem da claim `sub` (mapeada para `ClaimTypes.NameIdentifier`), nunca do e-mail.
- Autorização **fechada por padrão**: `FallbackPolicy` exige usuário autenticado; exceções explícitas com `AllowAnonymous` (`/health`, `/openapi`, Scalar).
- CORS sem `AllowCredentials`: token vai no header, não em cookie.
- A emissão do token (login/refresh) é escopo da TASK-002.

## Alternativas consideradas
- **Bearer opaco nativo do Identity** (`MapIdentityApi`): menos código, mas token proprietário e pouco controle sobre claims e expiração.
- **Cookie**: presume cliente navegador, contrário à API agnóstica de UI.

## Consequências
- A chave de assinatura é segredo: `dotnet user-secrets` em desenvolvimento, `.env` (não versionado) no compose, cofre em produção.
