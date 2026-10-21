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
- [x] Com credenciais válidas o administrador acessa o backoffice.
- [x] Credenciais inválidas exibem mensagem genérica e não autenticam.
- [x] Qualquer chamada à API sem autenticação é rejeitada.
- [x] Acessar uma tela sem sessão redireciona para o login.
- [x] Logout encerra a sessão e impede novo acesso sem login.
- [x] Sessão expirada exige novo login.
