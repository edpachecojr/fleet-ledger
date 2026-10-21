# Progresso da Implementação

Arquivo vivo: atualize ao iniciar, concluir ou bloquear uma tarefa, e ao tomar decisões.

**Legenda:** ⬜ Pendente · 🟨 Em andamento · ✅ Concluída · ⛔ Bloqueada

## Resumo

| Marco | Tarefas | Concluídas | Status |
|-------|---------|-----------:|--------|
| M0 — Fundação | 001–002 | 2/2 | ✅ |
| M1 — Cadastros | 003–007 | 0/5 | ⬜ |
| M2 — Financeiro base | 008–010 | 0/3 | ⬜ |
| M3 — Viagens | 011–018 | 0/8 | ⬜ |
| M4 — Faturamento | 019–024 | 0/6 | ⬜ |
| M5 — Manutenção | 025–027 | 0/3 | ⬜ |
| M6 — Resultados | 028–032 | 0/5 | ⬜ |
| **Total** | | **2/32** | |

## Tarefas

| ID | Tarefa | Status | Início | Conclusão | Observações |
|----|--------|--------|--------|-----------|-------------|
| TASK-001 | Estrutura base da aplicação executável | ✅ | 2026-10-20 | 2026-10-20 | API (`fleetledger-api/`) + backoffice (`fleetledger-backoffice-web/`) + banco com `docker compose up --build`. CI ainda não configurado. |
| TASK-002 | Login do administrador e proteção de acesso | ✅ | 2026-10-20 | 2026-10-20 | `POST /v1/auth/login` (JWT 8 h, bloqueio 15 min após 5 erros), seed do administrador, tela de login, rotas protegidas, logout e expiração no backoffice. ADR 0007. |
| TASK-003 | Plano de Contas padrão | ⬜ | | | |
| TASK-004 | Cadastro de veículos | ⬜ | | | |
| TASK-005 | Status operacional do veículo e bloqueio de alocação | ⬜ | | | |
| TASK-006 | Cadastro de clientes | ⬜ | | | |
| TASK-007 | Cadastro de motoristas | ⬜ | | | |
| TASK-008 | Contas a Pagar: títulos e consulta | ⬜ | | | |
| TASK-009 | Baixa de títulos do Contas a Pagar | ⬜ | | | |
| TASK-010 | Lançamento de despesas fixas da frota | ⬜ | | | |
| TASK-011 | Abertura de viagem | ⬜ | | | |
| TASK-012 | Consulta e detalhe de viagens | ⬜ | | | |
| TASK-013 | Edição e cancelamento de viagem aberta | ⬜ | | | |
| TASK-014 | Adiantamentos ao motorista | ⬜ | | | |
| TASK-015 | Lançamento de despesas da viagem | ⬜ | | | |
| TASK-016 | Cálculo da remuneração do motorista | ⬜ | | | |
| TASK-017 | Fechamento da viagem | ⬜ | | | |
| TASK-018 | Acerto final com o motorista | ⬜ | | | |
| TASK-019 | Fila de viagens aguardando faturamento | ⬜ | | | |
| TASK-020 | Geração de fatura | ⬜ | | | |
| TASK-021 | Contas a Receber e baixa de faturas | ⬜ | | | |
| TASK-022 | Cancelamento de fatura | ⬜ | | | |
| TASK-023 | Imutabilidade de viagens faturadas | ⬜ | | | |
| TASK-024 | Extrato de caixa | ⬜ | | | |
| TASK-025 | Planos de manutenção por veículo | ⬜ | | | |
| TASK-026 | Semáforo de alertas de manutenção | ⬜ | | | |
| TASK-027 | Registro de manutenção executada | ⬜ | | | |
| TASK-028 | Rateio de custo fixo diário | ⬜ | | | |
| TASK-029 | DRE da viagem | ⬜ | | | |
| TASK-030 | DRE mensal por veículo | ⬜ | | | |
| TASK-031 | Dashboard inicial: indicadores operacionais | ⬜ | | | |
| TASK-032 | Dashboard inicial: resultado financeiro do mês | ⬜ | | | |

## Decisões

Registre aqui as respostas às questões em aberto do [backlog](backlog/README.md#questões-em-aberto-validar-com-o-gestor) e demais decisões de produto/técnicas.

| Data | Ref. | Decisão | Tarefas afetadas |
|------|------|---------|------------------|
| 2026-10-20 | Q1 | Motorista com CNH/toxicológico vencido gera **apenas aviso** na abertura da viagem (sem bloqueio). | 007, 011 |
| 2026-10-20 | Q2 | Viagem fechada (não faturada) **não pode ser reaberta** no MVP. | 013, 017 |
| 2026-10-20 | Q3 | Dias da viagem para rateio = **dias corridos, inclusivos** (dia de saída e dia de chegada contam). | 017, 028 |
| 2026-10-20 | Q4 | "Veículos ativos" no rateio = veículos **não inativados no mês de competência**, independente do status operacional. | 028 |
| 2026-10-20 | Q5 | Saldo negativo no acerto gera **crédito da empresa a abater no próximo acerto** do motorista. | 018 |
| 2026-10-20 | Q6 | Despesa de viagem paga pela **empresa** → título no Contas a Pagar; paga pelo **motorista** → somada no acerto. | 015, 018 |
| 2026-10-20 | Q7 | Competência da viagem na DRE mensal = **data de fechamento** da viagem. | 030, 032 |
| 2026-10-20 | Q8 | Despesas fixas **não** são vinculadas a veículo; todas entram no rateio geral. | 010, 028 |
| 2026-10-20 | Q9 | Plano de Contas adotado conforme a **proposta da TASK-003**. | 003 |
| 2026-10-20 | Q10 | "Carregamento instantâneo" dos dashboards = **< 1 s** com massa de dados de referência. | 031, 032 |
| 2026-10-20 | ADR 0001 | Persistência: PostgreSQL + EF Core, `snake_case`, Id UUID v7 gerado na aplicação. | 001 |
| 2026-10-20 | ADR 0002 | Autenticação: ASP.NET Core Identity + JWT bearer; autorização fechada por padrão. | 001, 002 |
| 2026-10-20 | ADR 0003 | Isolamento lógico por organização: `IOrganizationOwned` + `ForOrganization` explícito; organização só via `IUserContext`. | todas |
| 2026-10-20 | ADR 0004 | Testes: só unitários com fakes, sem banco; integração ponta a ponta fica para depois. | todas |
| 2026-10-20 | ADR 0005 | Contrato HTTP: Minimal API + OpenAPI (Scalar em dev); front-end fora de `fleetledger-api`. | todas |
| 2026-10-20 | TASK-002 | Access token JWT de **8 h sem refresh token**; expirou, novo login. Logout só no cliente (descarta o token). | 002 |
| 2026-10-20 | TASK-002 | Login por **e-mail** + senha; **bloqueio de 15 min após 5 tentativas** erradas (lockout do Identity), sem revelar o bloqueio na mensagem. | 002 |
| 2026-10-20 | TASK-002 | Administrador e organização inicial criados na subida da API a partir da seção `Seed` da configuração (`.env` no compose); idempotente. | 002 |
| 2026-10-20 | TASK-002 | Backoffice guarda o token em **`sessionStorage`** (sobrevive ao F5, some ao fechar a aba). | 002 |
| 2026-10-20 | ADR 0007 | Sessão do administrador: JWT 8 h sem refresh, logout no cliente, lockout do Identity, seed por configuração, token em `sessionStorage`. | 002 |
| 2026-10-20 | ADR 0006 | Front-end: SPA React 19 + Vite, React Router, TanStack Query, cliente gerado do OpenAPI, design system Backoffice com Tailwind 4; nginx faz proxy de `/api`. | todas |

## Diário

Registro cronológico do que foi feito, problemas e próximos passos.

### 2026-10-20
- Backlog inicial criado a partir da especificação do MVP (32 tarefas, 6 épicos), roadmap em 7 marcos e este arquivo de acompanhamento.
- Questões em aberto Q1–Q10 decididas: propostas do backlog aceitas pelo gestor (ver Decisões); tarefas afetadas atualizadas.
- **Próximo passo:** definir os detalhes técnicos e iniciar a TASK-001.
- TASK-001 iniciada — back-end: solução `fleetledger-api/` (.NET 10, `FleetLedger.Core` + `FleetLedger.Api` + testes unitários), PostgreSQL + EF Core com migration `InitialSchema` (`organizations` + tabelas do Identity sem prefixo `AspNet`), Identity + JWT, isolamento por organização (`IOrganizationOwned`, `ForOrganization`, `IUserContext` com `HybridCache`), Serilog + OpenTelemetry, OpenAPI + Scalar, `/health` com verificação do banco (200/503). `docker compose up --build` sobe banco + API; `dotnet test` roda 78 testes verdes. Decisões em `docs/adr/0001`–`0005`; regras em `CLAUDE.md` (raiz) e `fleetledger-api/CLAUDE.md`.
- Observação: o PostgreSQL do compose usa a porta 5433 do host (a 5432 já estava ocupada nesta máquina).
- Projeto renomeado de "Iowa" para **FleetLedger**, nome descritivo para publicação no GitHub ("fleet" = operação da frota; "ledger" = financeiro). Pasta `fleetledger-api/`, solução `FleetLedger.slnx`, namespaces `FleetLedger.*`, banco/usuário `fleetledger`.
- **Próximo passo:** front-end da TASK-001 (shell do backoffice) e incluí-lo no compose; depois TASK-002 (emissão do JWT, seed do administrador e da organização).
- TASK-001 concluída — front-end: `fleetledger-backoffice-web/` (React 19.3, Vite 8, TypeScript 6, Tailwind 4, React Router 8, TanStack Query 5, pnpm). Shell do design system Backoffice (sidebar, header, painel de aparência com modo/cor/densidade/direção, temas claro e escuro), página "Visão geral" com card de status do sistema lendo `GET /health` pelo cliente gerado do OpenAPI. 30 testes Vitest verdes; `docker compose up --build` sobe banco + API + backoffice (http://localhost:3000). Na API, `/health` virou endpoint Minimal API documentado no OpenAPI (status como enum texto, números estritos).
- **Próximo passo:** TASK-002 — emissão do JWT, seed do administrador e da organização, tela de login e `AccessTokenSource` da sessão no backoffice.
- TASK-002 iniciada e concluída. API: `POST /v1/auth/login` (anônima, `ValidationFilter`), `AccessTokenIssuer` (JWT só com `sub` e `jti`, vida em `Jwt:AccessTokenLifetimeMinutes`), `IdentityCredentialVerifier` com lockout (5 erros → 15 min) e 401 genérico para qualquer recusa, `AdministratorSeeder` idempotente lendo `AdministratorSeed:*` (compose: `ADMIN_*` no `.env`). Backoffice: `AdminSession` (sessionStorage) como `AccessTokenSource`, `RequireSession` protegendo todas as rotas menos `/login`, tela de login com `TextField` do design system, botão "Sair" no header, fim da sessão no `expiresAt` e em 401 de requisição autenticada, cache do TanStack Query limpo ao sair. `dotnet test`: 116 testes; `pnpm test`: 69 testes. Validado ponta a ponta no compose (login certo/errado, token válido/adulterado, seed não repete).
- CA1873 desligada junto com a CA1848 no `.editorconfig` da API (mesma decisão: logs com message template, sem `LoggerMessage`).
- Observação: o login com e-mail inexistente responde mais rápido que com senha errada (não calcula hash). Com um único administrador o risco de enumeração é baixo; revisitar se houver cadastro de usuários.
- **Marco M0 concluído.** **Próximo passo:** M1 — TASK-003 (Plano de Contas padrão).
