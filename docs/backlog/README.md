# Backlog — Sistema de Gestão Operacional e Financeira para Transportadoras (MVP)

Backlog derivado da especificação do MVP. Cada tarefa é uma **funcionalidade testável de ponta a ponta** (regra de negócio + API + interface), com critérios de aceite verificáveis. Detalhes técnicos ficam de fora de propósito: serão definidos no início de cada tarefa.

- Ordem de execução e marcos: [`../ROADMAP.md`](../ROADMAP.md)
- Estado da implementação: [`../PROGRESS.md`](../PROGRESS.md)

## Estrutura

```
docs/backlog/
├── E0-fundacao/      Base executável e acesso
├── E1-cadastros/     Plano de contas, veículos, clientes, motoristas
├── E2-viagens/       Abertura, adiantamentos, despesas, fechamento e acerto
├── E3-financeiro/    Contas a pagar, despesas fixas, faturamento, contas a receber, caixa
├── E4-manutencao/    Planos, semáforo e registro de manutenções
└── E5-resultados/    Rateio, DRE da viagem, DRE por veículo e dashboards
```

A numeração das tarefas (`TASK-NNN`) segue a **ordem sugerida de execução**, não o épico. Por isso algumas tarefas do E3 (Contas a Pagar) vêm antes do E2: viagens dependem delas.

## Modelo de tarefa

Cada arquivo contém: **Objetivo**, **Escopo**, **Regras de negócio**, **Critérios de aceite** (checklist) e **Dependências**. Itens marcados **a decidir / a validar** dependem de uma decisão listada abaixo.

**Definição de Pronto (DoD)** — uma tarefa só é concluída quando:
1. Todos os critérios de aceite estão atendidos.
2. Existem testes automatizados cobrindo as regras de negócio.
3. A funcionalidade está acessível pela interface (quando aplicável).
4. O [`PROGRESS.md`](../PROGRESS.md) foi atualizado.

## Índice

| ID | Tarefa | Épico | Rastreabilidade |
|----|--------|-------|-----------------|
| [TASK-001](E0-fundacao/TASK-001-estrutura-base-aplicacao.md) | Estrutura base da aplicação executável | E0 | — |
| [TASK-002](E0-fundacao/TASK-002-login-administrador.md) | Login do administrador e proteção de acesso | E0 | RNF02 |
| [TASK-003](E1-cadastros/TASK-003-plano-de-contas-padrao.md) | Plano de Contas padrão | E1 | RF10 |
| [TASK-004](E1-cadastros/TASK-004-cadastro-veiculos.md) | Cadastro de veículos | E1 | RF01, US01, US02 |
| [TASK-005](E1-cadastros/TASK-005-status-operacional-veiculo.md) | Status operacional do veículo e bloqueio de alocação | E1 | RF01, RF08, US03 |
| [TASK-006](E1-cadastros/TASK-006-cadastro-clientes.md) | Cadastro de clientes | E1 | RF03, US04 |
| [TASK-007](E1-cadastros/TASK-007-cadastro-motoristas.md) | Cadastro de motoristas | E1 | RF02, US05 |
| [TASK-008](E3-financeiro/TASK-008-contas-a-pagar-titulos.md) | Contas a Pagar: títulos e consulta | E3 | RF05, US13 |
| [TASK-009](E3-financeiro/TASK-009-baixa-contas-a-pagar.md) | Baixa de títulos do Contas a Pagar | E3 | US13 |
| [TASK-010](E3-financeiro/TASK-010-despesas-fixas.md) | Lançamento de despesas fixas da frota | E3 | US12, RF13 |
| [TASK-011](E2-viagens/TASK-011-abertura-viagem.md) | Abertura de viagem | E2 | RF04, RF08, US06, US07 |
| [TASK-012](E2-viagens/TASK-012-consulta-viagens.md) | Consulta e detalhe de viagens | E2 | RNF01 |
| [TASK-013](E2-viagens/TASK-013-edicao-cancelamento-viagem.md) | Edição e cancelamento de viagem aberta | E2 | RF04, RNF03 |
| [TASK-014](E2-viagens/TASK-014-adiantamentos-motorista.md) | Adiantamentos ao motorista | E2 | RF05, US08 |
| [TASK-015](E2-viagens/TASK-015-despesas-viagem.md) | Lançamento de despesas da viagem | E2 | US09, RNF01 |
| [TASK-016](E2-viagens/TASK-016-remuneracao-motorista.md) | Cálculo da remuneração do motorista | E2 | RF06 |
| [TASK-017](E2-viagens/TASK-017-fechamento-viagem.md) | Fechamento da viagem | E2 | RF07, US10 |
| [TASK-018](E2-viagens/TASK-018-acerto-motorista.md) | Acerto final com o motorista | E2 | US11 |
| [TASK-019](E3-financeiro/TASK-019-fila-faturamento.md) | Fila de viagens aguardando faturamento | E3 | US14 |
| [TASK-020](E3-financeiro/TASK-020-geracao-fatura.md) | Geração de fatura | E3 | RF09, US15 |
| [TASK-021](E3-financeiro/TASK-021-contas-a-receber-baixa.md) | Contas a Receber e baixa de faturas | E3 | US16 |
| [TASK-022](E3-financeiro/TASK-022-cancelamento-fatura.md) | Cancelamento de fatura | E3 | RF09, RNF03 |
| [TASK-023](E3-financeiro/TASK-023-imutabilidade-viagens-faturadas.md) | Imutabilidade de viagens faturadas | E3 | RNF03 |
| [TASK-024](E3-financeiro/TASK-024-extrato-caixa.md) | Extrato de caixa | E3 | US13, US16 |
| [TASK-025](E4-manutencao/TASK-025-planos-manutencao.md) | Planos de manutenção por veículo | E4 | RF11, US17 |
| [TASK-026](E4-manutencao/TASK-026-semaforo-manutencao.md) | Semáforo de alertas de manutenção | E4 | RF11, US18 |
| [TASK-027](E4-manutencao/TASK-027-registro-manutencao.md) | Registro de manutenção executada | E4 | US19 |
| [TASK-028](E5-resultados/TASK-028-rateio-custo-fixo.md) | Rateio de custo fixo diário | E5 | RF13 |
| [TASK-029](E5-resultados/TASK-029-dre-viagem.md) | DRE da viagem | E5 | RF12, US20 |
| [TASK-030](E5-resultados/TASK-030-dre-mensal-veiculo.md) | DRE mensal por veículo | E5 | US21 |
| [TASK-031](E5-resultados/TASK-031-dashboard-operacional.md) | Dashboard inicial: indicadores operacionais | E5 | US18, RNF04 |
| [TASK-032](E5-resultados/TASK-032-dashboard-financeiro.md) | Dashboard inicial: resultado financeiro do mês | E5 | US22, RNF04 |

## Cobertura da especificação

| Requisito | Tarefas |
|-----------|---------|
| RF01 | 004, 005 |
| RF02 | 007, 031 |
| RF03 | 006 |
| RF04 | 011, 013 |
| RF05 | 008, 014 |
| RF06 | 016, 018 |
| RF07 | 017 |
| RF08 | 005, 011 |
| RF09 | 020, 022 |
| RF10 | 003 |
| RF11 | 025, 026 |
| RF12 | 029, 030 |
| RF13 | 010, 028 |
| RNF01 | 009, 012, 015, 018 |
| RNF02 | 002 |
| RNF03 | 013, 022, 023, 028 |
| RNF04 | 031, 032 |
| US01–US05 | 004–007 |
| US06–US11 | 011, 014–018 |
| US12–US16 | 010, 009, 019–021 |
| US17–US19 | 025–027 |
| US20–US22 | 029, 030, 032 |

## Questões em aberto (validar com o gestor)

| # | Questão | Proposta atual | Afeta |
|---|---------|----------------|-------|
| Q1 | Motorista com CNH/toxicológico vencido: bloquear ou só avisar na abertura da viagem? | Apenas avisar | 007, 011 |
| Q2 | Viagem fechada (não faturada) pode ser reaberta? | Não no MVP | 013, 017 |
| Q3 | Como contar os dias da viagem para o rateio? | Dias corridos, inclusivos (saída e chegada) | 017, 028 |
| Q4 | Definição de "veículos ativos" no rateio | Veículos não inativados no mês, independente do status operacional | 028 |
| Q5 | Saldo negativo no acerto (adiantamento > remuneração + despesas) | Gerar crédito a abater no próximo acerto do motorista | 018 |
| Q6 | Despesas de viagem: quem pagou (empresa × motorista) | Empresa → título no Contas a Pagar; motorista → somada no acerto | 015, 018 |
| Q7 | Competência da viagem na DRE mensal | Data de fechamento da viagem | 030, 032 |
| Q8 | Despesas fixas vinculadas a um veículo específico (ex.: IPVA de um caminhão)? | Não; todas entram no rateio geral | 010, 028 |
| Q9 | Lista definitiva de contas do Plano de Contas | Proposta na TASK-003 | 003 |
| Q10 | Meta numérica de "carregamento instantâneo" | < 1 s com massa de referência | 031, 032 |
