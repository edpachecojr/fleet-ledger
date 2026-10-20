# Roadmap — MVP Gestão de Transportadoras

O roadmap organiza as 32 tarefas do [backlog](backlog/README.md) em **marcos incrementais**. Ao fim de cada marco o sistema está utilizável e demonstrável para o gestor.

```
M0 Fundação ─► M1 Cadastros ─► M2 Financeiro base ─► M3 Operação de viagens ─► M4 Faturamento ─► M5 Manutenção ─► M6 Resultados (DRE)
                                                                                   ▲
                                                     M5 pode iniciar após M1 (paralelo a M2–M4) ─┘
```

---

## M0 — Fundação
**Objetivo:** aplicação rodando, com acesso restrito ao administrador.

| Tarefa | Descrição |
|--------|-----------|
| TASK-001 | Estrutura base da aplicação executável |
| TASK-002 | Login do administrador e proteção de acesso |

**Entrega:** o administrador faz login e vê o backoffice vazio.

---

## M1 — Cadastros e Parâmetros Base
**Objetivo:** dados mestres da operação disponíveis.

| Tarefa | Descrição |
|--------|-----------|
| TASK-003 | Plano de Contas padrão |
| TASK-004 | Cadastro de veículos |
| TASK-005 | Status operacional do veículo e bloqueio de alocação |
| TASK-006 | Cadastro de clientes |
| TASK-007 | Cadastro de motoristas |

**Entrega:** frota, clientes e motoristas cadastrados; veículos podem ser postos em manutenção.

---

## M2 — Financeiro base (Contas a Pagar)
**Objetivo:** núcleo financeiro que receberá os lançamentos gerados pelas viagens e manutenções.

| Tarefa | Descrição |
|--------|-----------|
| TASK-008 | Contas a Pagar: títulos e consulta |
| TASK-009 | Baixa de títulos do Contas a Pagar |
| TASK-010 | Lançamento de despesas fixas da frota |

**Entrega:** o gestor controla contas a pagar e já registra os custos fixos mensais.

---

## M3 — Operação e Acerto de Viagens
**Objetivo:** ciclo completo da viagem, da saída ao acerto com o motorista.

| Tarefa | Descrição |
|--------|-----------|
| TASK-011 | Abertura de viagem |
| TASK-012 | Consulta e detalhe de viagens |
| TASK-013 | Edição e cancelamento de viagem aberta |
| TASK-014 | Adiantamentos ao motorista |
| TASK-015 | Lançamento de despesas da viagem |
| TASK-016 | Cálculo da remuneração do motorista |
| TASK-017 | Fechamento da viagem |
| TASK-018 | Acerto final com o motorista |

**Entrega:** a operação diária roda no sistema; hodômetro e provisão de manutenção atualizados automaticamente.

---

## M4 — Faturamento e Contas a Receber
**Objetivo:** cobrar os clientes e fechar o fluxo de caixa.

| Tarefa | Descrição |
|--------|-----------|
| TASK-019 | Fila de viagens aguardando faturamento |
| TASK-020 | Geração de fatura |
| TASK-021 | Contas a Receber e baixa de faturas |
| TASK-022 | Cancelamento de fatura |
| TASK-023 | Imutabilidade de viagens faturadas |
| TASK-024 | Extrato de caixa |

**Entrega:** ciclo financeiro completo (pagar, faturar, receber) com histórico protegido.

---

## M5 — Controle de Manutenção (Semáforo)
**Objetivo:** alertas preventivos automáticos e registro do custo real de oficina.

| Tarefa | Descrição |
|--------|-----------|
| TASK-025 | Planos de manutenção por veículo |
| TASK-026 | Semáforo de alertas de manutenção |
| TASK-027 | Registro de manutenção executada |

**Entrega:** o gestor vê o que está para vencer e registra as manutenções realizadas.

> Pode ser desenvolvido em paralelo a M2–M4 (depende apenas de M1; TASK-026 integra com o fechamento da TASK-017 e TASK-027 com o Contas a Pagar da TASK-008).

---

## M6 — Resultados Econômicos (DRE e Dashboards)
**Objetivo:** transformar os dados operacionais em resultado econômico.

| Tarefa | Descrição |
|--------|-----------|
| TASK-028 | Rateio de custo fixo diário |
| TASK-029 | DRE da viagem |
| TASK-030 | DRE mensal por veículo |
| TASK-031 | Dashboard inicial: indicadores operacionais |
| TASK-032 | Dashboard inicial: resultado financeiro do mês |

**Entrega:** MVP completo — margem por viagem, resultado por caminhão e lucro mensal na tela inicial.

---

## Critério de conclusão do MVP
- Todas as tarefas concluídas conforme a Definição de Pronto do [backlog](backlog/README.md#modelo-de-tarefa).
- Todos os RF, RNF e US da especificação cobertos (ver tabela de cobertura no backlog).
- Questões em aberto (Q1–Q10) decididas e registradas no [PROGRESS.md](PROGRESS.md).
- Roteiro de aceite ponta a ponta executado: cadastrar → abrir viagem → adiantar → lançar despesas → fechar → acertar → faturar → receber → registrar manutenção → conferir DRE e dashboard.
