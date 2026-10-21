# ADR 0007 — Sessão do administrador: JWT de 8 h sem refresh, guardado em sessionStorage

**Data:** 2026-10-20 · **Status:** aceito · **Complementa:** [0002](0002-autenticacao-identity-jwt.md)

## Contexto
A TASK-002 entrega o login do único administrador. O ADR 0002 definiu Identity + JWT bearer e deixou para esta tarefa a emissão do token, a duração da sessão, a criação do administrador e onde o backoffice guarda o token.

## Decisão
- **Emissão:** `POST /v1/auth/login` (anônima) devolve `IssuedAccessToken { accessToken, expiresAt }` no envelope `Response<T>`. O JWT leva só `sub` (id do usuário) e `jti`; a organização continua resolvida no servidor (ADR 0003) e nenhum e-mail vai em claim.
- **Duração:** `Jwt:AccessTokenLifetimeMinutes`, padrão **480 (8 h)**, entre 5 e 1440. **Sem refresh token**: expirou, novo login.
- **Logout só no cliente:** o backoffice descarta o token. Ele continua válido na API até expirar.
- **Bloqueio:** lockout do Identity, **15 min após 5 senhas erradas**. Toda recusa (e-mail inexistente, senha errada, bloqueio) responde 401 com a mesma mensagem; o motivo só vai para o log.
- **Administrador inicial:** criado na subida da API a partir de `AdministratorSeed:*` (no compose, `ADMIN_*` do `.env`), junto com a organização, **só se não houver nenhum usuário**. Mudar a configuração depois não cria outro administrador nem troca a senha.
- **Backoffice:** o token fica em **`sessionStorage`** (`AdminSession`). Sobrevive ao F5 e some ao fechar a aba. A sessão acaba sozinha no `expiresAt` e também quando a API responde 401 a uma requisição que levou token.

## Alternativas consideradas
- **Refresh token:** sessões longas sem novo login, mas exige armazenamento e revogação no servidor; desproporcional para um único usuário no MVP.
- **Token só em memória:** mais resistente a XSS, mas o F5 derruba a sessão.
- **`localStorage`:** a sessão sobreviveria ao fechar o navegador, contrário ao critério "sessão expirada exige novo login" num computador compartilhado.
- **Cookie HttpOnly:** protege contra XSS, mas presume navegador e traz CSRF; contrário à API agnóstica de UI (ADR 0002).
- **Revogação no logout** (lista de bloqueio ou `SecurityStamp` validado a cada requisição): custo de uma consulta por chamada; adiada até existir mais de um usuário.

## Consequências
- Um token vazado vale até 8 h. Mitigação: vida curta, HTTPS e nenhum dado sensível no token.
- Troca de senha ou remoção do usuário não derruba tokens já emitidos até expirarem (`UserNotFoundException` cobre usuário removido).
- Para trocar a senha do administrador no MVP é preciso agir no banco: não há tela de usuário.
