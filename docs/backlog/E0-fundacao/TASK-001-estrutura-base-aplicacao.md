# TASK-001 — Estrutura base da aplicação executável

**Épico:** E0 — Fundação · **Rastreabilidade:** — · **Depende de:** —

## Objetivo
Ter o monorepo com backend e frontend mínimos rodando localmente, prontos para receber as funcionalidades.

## Escopo
- Estrutura do monorepo (backend, frontend, testes, docs).
- Backend respondendo a um endpoint de verificação de saúde.
- Frontend com o "shell" do backoffice (layout, menu lateral vazio, página inicial placeholder).
- Banco de dados provisionado e migrações iniciais executando.
- Pipeline de build e testes automatizados executando localmente (e em CI, se aplicável).

## Critérios de aceite
- [x] Um comando documentado sobe backend, frontend e banco localmente.
- [x] O endpoint de saúde retorna sucesso e indica conectividade com o banco.
- [x] O frontend carrega o layout base no navegador desktop.
- [x] A suíte de testes roda e passa (ao menos um teste de fumaça por projeto).
- [x] README com instruções de execução.

## Observações
Detalhes técnicos (stack, ferramentas, padrões) serão definidos pelo time antes do início.
