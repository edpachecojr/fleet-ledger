# Progresso da Implementação

Arquivo vivo: atualize ao iniciar, concluir ou bloquear uma tarefa, e ao tomar decisões.

**Legenda:** ⬜ Pendente · 🟨 Em andamento · ✅ Concluída · ⛔ Bloqueada

## Resumo

| Marco | Tarefas | Concluídas | Status |
|-------|---------|-----------:|--------|
| M0 — Fundação | 001–002 | 0/2 | ⬜ |
| M1 — Cadastros | 003–007 | 0/5 | ⬜ |
| M2 — Financeiro base | 008–010 | 0/3 | ⬜ |
| M3 — Viagens | 011–018 | 0/8 | ⬜ |
| M4 — Faturamento | 019–024 | 0/6 | ⬜ |
| M5 — Manutenção | 025–027 | 0/3 | ⬜ |
| M6 — Resultados | 028–032 | 0/5 | ⬜ |
| **Total** | | **0/32** | |

## Tarefas

| ID | Tarefa | Status | Início | Conclusão | Observações |
|----|--------|--------|--------|-----------|-------------|
| TASK-001 | Estrutura base da aplicação executável | ⬜ | | | |
| TASK-002 | Login do administrador e proteção de acesso | ⬜ | | | |
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

## Diário

Registro cronológico do que foi feito, problemas e próximos passos.

### 2026-10-20
- Backlog inicial criado a partir da especificação do MVP (32 tarefas, 6 épicos), roadmap em 7 marcos e este arquivo de acompanhamento.
- Questões em aberto Q1–Q10 decididas: propostas do backlog aceitas pelo gestor (ver Decisões); tarefas afetadas atualizadas.
- **Próximo passo:** definir os detalhes técnicos e iniciar a TASK-001.
