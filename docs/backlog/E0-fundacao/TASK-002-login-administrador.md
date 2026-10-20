# TASK-002 — Login do administrador e proteção de acesso

**Épico:** E0 — Fundação · **Rastreabilidade:** RNF02 · **Depende de:** TASK-001

## Objetivo
Restringir todo o backoffice a um único usuário Administrador (gestor da frota).

## Escopo
- Tela de login (usuário/senha) e logout.
- Usuário administrador único criado via configuração/seed (sem cadastro de usuários no MVP).
- Proteção de todas as rotas da API e telas do frontend.
- Expiração de sessão e redirecionamento para login.

## Regras de negócio
- Não existe autocadastro nem múltiplos perfis no MVP.
- Senha armazenada de forma segura (nunca em texto puro).

## Critérios de aceite
- [ ] Com credenciais válidas o administrador acessa o backoffice.
- [ ] Credenciais inválidas exibem mensagem genérica e não autenticam.
- [ ] Qualquer chamada à API sem autenticação é rejeitada.
- [ ] Acessar uma tela sem sessão redireciona para o login.
- [ ] Logout encerra a sessão e impede novo acesso sem login.
- [ ] Sessão expirada exige novo login.
