# Progresso da Implementação

Arquivo vivo: atualize ao iniciar, concluir ou bloquear uma tarefa, e ao tomar decisões.

**Legenda:** ⬜ Pendente · 🟨 Em andamento · ✅ Concluída · ⛔ Bloqueada

## Resumo

| Marco | Tarefas | Concluídas | Status |
|-------|---------|-----------:|--------|
| M0 — Fundação | 001–002 | 2/2 | ✅ |
| M1 — Cadastros | 003–007 | 3/5 | 🟨 |
| M2 — Financeiro base | 008–010 | 0/3 | ⬜ |
| M3 — Viagens | 011–018 | 0/8 | ⬜ |
| M4 — Faturamento | 019–024 | 0/6 | ⬜ |
| M5 — Manutenção | 025–027 | 0/3 | ⬜ |
| M6 — Resultados | 028–032 | 0/5 | ⬜ |
| **Total** | | **5/32** | |

## Tarefas

| ID | Tarefa | Status | Início | Conclusão | Observações |
|----|--------|--------|--------|-----------|-------------|
| TASK-001 | Estrutura base da aplicação executável | ✅ | 2026-10-20 | 2026-10-20 | API (`fleetledger-api/`) + backoffice (`fleetledger-backoffice-web/`) + banco com `docker compose up --build`. CI ainda não configurado. |
| TASK-002 | Login do administrador e proteção de acesso | ✅ | 2026-10-20 | 2026-10-20 | `POST /v1/auth/login` (JWT 8 h, bloqueio 15 min após 5 erros), seed do administrador, tela de login, rotas protegidas, logout e expiração no backoffice. ADR 0007. |
| TASK-003 | Plano de Contas padrão | ✅ | 2026-10-21 | 2026-10-21 | 7 grupos e 18 contas globais gravados pela migration `AddChartOfAccounts`; `GET /v1/ledger-accounts` (hierarquia) e `GET /v1/ledger-accounts/selectable?entryContext=` (seleção por contexto); tela "Plano de contas" somente leitura. |
| TASK-004 | Cadastro de veículos | ✅ | 2026-10-21 | 2026-10-21 | CRUD em `/v1/vehicles` (sem DELETE: inativar/reativar), placa antiga/Mercosul única por organização, busca por placa/modelo, `allocatable=true` para seleção de nova viagem; telas `/veiculos`, `/veiculos/novo`, `/veiculos/:id`. |
| TASK-005 | Status operacional do veículo e bloqueio de alocação | ✅ | 2026-10-21 | 2026-10-21 | `PUT /v1/vehicles/{id}/status` (Disponível ↔ Em manutenção; Em viagem 400; veículo em viagem ou inativo 409), filtro `status` na listagem, `VehicleAllocationRule` reutilizável; ações e badges no backoffice. |
| TASK-006 | Cadastro de clientes | 🟨 | 2026-10-21 | | |
| TASK-007 | Cadastro de motoristas | ✅ | 2026-10-21 | 2026-10-21 | CRUD em `/v1/drivers` (sem DELETE), CPF válido e único (não editável), CNH única, comissão 0–100% com 2 casas, situação documental calculada no servidor com "hoje" em America/Sao_Paulo e filtro `documentStatus`; telas `/motoristas`, `/motoristas/novo`, `/motoristas/:id`. |
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
| 2026-10-21 | M1 | Handlers dependem de um **store por módulo** (`I<Entidade>Store` em `Api/Data/Stores/<Modulo>/`, implementação EF com `ForOrganization`); testes de handler usam `Fake<Entidade>Store` e o SQL do store é conferido offline com `ToQueryString` (ADR 0004). | 003–007 |
| 2026-10-21 | TASK-003 | Plano de Contas **global** (não pertence a organização), gravado pela própria migration (`HasData`); códigos hierárquicos (`1`, `1.01`…) na ordem da DRE. | 003 |
| 2026-10-21 | TASK-003 | Contexto de lançamento manual por conta: **Viagem** (combustível, pedágio, chapas/descarga, alimentação/diárias, outras despesas de viagem), **Despesa fixa** (seguro, IPVA/licenciamento, aluguel, rastreamento, salários fixos, outros custos fixos), **Manutenção** (manutenção realizada), **Faturamento** (frete, descontos concedidos, acréscimos cobrados). Sistêmicas: comissão/remuneração, provisão de manutenção, adiantamento a motorista. | 003, 010, 015, 020, 027 |
| 2026-10-21 | M1 | Veículos, clientes e motoristas inativados **podem ser reativados** (ação "Reativar"). | 004, 006, 007 |
| 2026-10-21 | TASK-004 | Capacidade do veículo em **kg** (inteiro); **placa não é editável** depois do cadastro. | 004 |
| 2026-10-21 | TASK-006 | Cliente: só razão social, CNPJ e ciclo são obrigatórios; nome fantasia, telefone, e-mail e prazo são opcionais (validados quando informados). | 006 |
| 2026-10-21 | TASK-007 | Motorista: categoria da CNH em lista fixa (A, B, C, D, E, AB, AC, AD, AE), número da CNH com 11 dígitos, telefone obrigatório, dados bancários/PIX em texto livre opcional. Situação documental: **Vencido** se alguma validade < hoje; **Vencendo** se a mais próxima vence entre hoje e hoje+30 dias (inclusive); senão **Regular**; vale o pior documento, calculado no servidor. | 007, 011 |
| 2026-10-21 | M1 | Cadastro e edição em **páginas próprias** (`/veiculos`, `/veiculos/novo`, `/veiculos/:id`…), formulário num Card como no card FormControls. | 004, 006, 007 |
| 2026-10-21 | TASK-004 | Hodômetro em **km inteiro**; taxa de provisão em **R$/km com 2 casas**; veículo pode ser **inativado em qualquer status** (o status é mantido). | 004, 017, 028 |
| 2026-10-21 | TASK-005 | Veículo **inativo não muda de status operacional**: reative antes. | 005 |
| 2026-10-21 | M1 | "Hoje" (vencimentos, datas) é calculado no fuso **America/Sao_Paulo**, configurável na API. | 007, todas |
| 2026-10-21 | M1 | **CNPJ do cliente e CPF do motorista não são editáveis** depois do cadastro (como a placa). Telefone: DDD + número, guardado só com dígitos (10 ou 11). | 006, 007 |
| 2026-10-21 | TASK-007 | Validade do exame toxicológico **opcional** (em branco não conta na situação documental); **número da CNH único** por organização; comissão padrão com **2 casas** (0% a 100%). | 007, 016 |

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

### 2026-10-21
- Commits da TASK-002 feitos (`bc03fff`, `32c3f7b`); o push ficou pendente: o WSL não tem credencial do GitHub. Decisão: seguir commitando por tarefa e o push sai quando houver credencial.
- Dúvidas da M1 respondidas pelo gestor (ver Decisões de 2026-10-21).
- TASK-003 iniciada.
- TASK-003 concluída. API: `LedgerAccountGroup`/`LedgerAccount` (globais, sem organização) com `ChartOfAccountsCatalog` gravado por `HasData` na migration `AddChartOfAccounts`; contexto de lançamento manual em `ELedgerEntryContext` (nulo = sistêmica). Endpoints só de leitura: `GET /v1/ledger-accounts` e `GET /v1/ledger-accounts/selectable?entryContext=` (POST responde 405). Primeiro módulo com o padrão de store (`ILedgerAccountStore` + `FakeLedgerAccountStore`); `CLAUDE.md` da API atualizado. Backoffice: componente `DataTable` do design system e página "Plano de contas" (um Card por grupo, conta sistêmica com badge "Sistêmica"), item no menu "Financeiro". `dotnet test`: 132 testes; `pnpm test`: 83 testes. Conferido no compose.
- Correção: parâmetro de query ausente/inválido (`BadHttpRequestException`) respondia 500; agora o `UseExceptionHandler` usa o status da exceção (400). Teste de regressão em `HttpContractExtensionTests`.
- As listas de seleção de conta no backoffice serão montadas nas telas que lançam (TASK-010, 015, 020, 027), consumindo `/selectable`.
- TASK-004 iniciada.
- TASK-004 concluída. API: `Vehicle` (`IOrganizationOwned`, placa normalizada imutável, hodômetro atual só pelo sistema, `DeactivatedAt` + `IsActive` derivado), migration `AddVehicles` com índice único `organization_id + plate`; `VehicleStore` com busca ILIKE escapada (`SearchPattern`) e paginação limitada no servidor (`PagedRequest.Effective*`); `VehicleAllocationRule` (ativo e Disponível) usada pelo filtro `allocatable`; `ByIdRequest` genérico para rotas por id. Backoffice: componentes `TextField` com erro, `SelectField`, `ToolbarSearch`, `TablePagination`, `ConfirmDialog`, `ButtonLink`, `InlineMessage`; `useMutationForm` + `outcomeOf` (400 por campo, 404/409 com mensagem) e `listQuery` (busca, situação, página) para os próximos cadastros; menu "Cadastros › Veículos" ativo também nas subrotas. `dotnet test`: 192 testes; `pnpm test`: 146 testes. Conferido no compose (201, 409, 400 por campo, busca, inativação fora de `allocatable`, DELETE 405).
- A seleção de veículo na abertura de viagem (TASK-011) vai consumir `GET /v1/vehicles?allocatable=true`.
- TASK-005 iniciada.
- TASK-005 concluída. API: `ChangeVehicleStatusRequest` + validador (só Available/InMaintenance como destino manual), máquina de estados `VehicleStatusTransition` (inativo → 409 "reative antes", em viagem → 409, mesmo status → sem gravar), filtro `status` em `GET /v1/vehicles`. Backoffice: ações "Colocar em manutenção" / "Liberar para uso" no detalhe (nenhuma em viagem ou inativo), filtro de status na lista (`ToolbarFilter`), badge de status com cor de status (verde, azul, âmbar). `dotnet test`: 208 testes; `pnpm test`: 156 testes. Conferido no compose.
- Critério "veículo Em Manutenção é rejeitado na abertura de viagem (API)": a regra (`VehicleAllocationRule`) e a lista de elegíveis (`allocatable=true`) estão prontas; a recusa no `POST` de viagem entra com a TASK-011, que a reutiliza.
- Refactor antes dos próximos cadastros: `IDeactivatable` + `ActivityChange` na API; `ActivityToggle`, `FormCard` e `SummaryList` no design system (veículos já usam).
- TASK-006 iniciada.
- TASK-007 iniciada.
- TASK-007 concluída. API: `Driver` (`IOrganizationOwned`, `IDeactivatable`), `ECnhCategory`, `EDocumentStatus`, `CpfNumber` (módulo 11); `DriverDocumentStatusRule` com a regra em memória e em expressão para o SQL (janela de 30 dias inclusiva, pior dos dois documentos, toxicológico em branco não conta); `DocumentStatus` calculado a cada resposta e fora do banco; seção `Calendar:TimeZone` (padrão America/Sao_Paulo, validada no boot) + `IBusinessCalendar`; migration `AddDrivers` com índices únicos `organization_id + cpf` e `organization_id + cnh_number`. Backoffice: lista com situação documental (Vencido/Vencendo/Regular), filtro por documentos, cadastro e detalhe (resumo "Documentos", CPF bloqueado), `TextAreaField`, `formatDateOnly` e `formatPercent`. `dotnet test`: 325 testes; `pnpm test`: 211 testes. Conferido no compose (fuso disponível no container, 409 de CNH duplicada, filtros).
- Registros de teste criados nas verificações ao vivo (2 veículos, 1 cliente, 1 motorista) removidos do banco local do compose.
- Push feito com o Git Credential Manager do Windows (`442f4aa..adbdbe2`); os commits das tarefas subiram juntos porque os pushes anteriores falharam por falta de credencial.
- Conferência visual das telas da M1 com Chromium headless (claro, escuro, 1440 px e 375 px). Dois desvios do card DataTable corrigidos: a página rolava de lado no celular (o `<main>` deixava o card crescer com a tabela) e as colunas secundárias não sumiam abaixo de 1024 px; no Plano de contas, as colunas ficaram com largura fixa para os grupos alinharem. Dados de exemplo usados nas capturas removidos do banco local.
- **Marco M1 concluído.** **Próximo passo:** M2 — TASK-008 (Contas a Pagar).
