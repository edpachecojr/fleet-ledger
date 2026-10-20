# TASK-028 — Rateio de custo fixo diário

**Épico:** E5 — Resultados Econômicos · **Rastreabilidade:** RF13, RF12 · **Depende de:** TASK-010, TASK-017

## Objetivo
Calcular o custo fixo diário por veículo e aplicá-lo aos dias de duração de cada viagem.

## Escopo
- Cálculo mensal: **custo fixo diário por veículo = (Total de custos fixos do mês / Veículos ativos no mês) / 30**.
- Rateio da viagem = Σ (custo diário do mês de cada dia) × dias da viagem — viagens que atravessam meses usam a taxa de cada mês.
- Consulta da memória de cálculo do mês: total fixo, veículos ativos, custo diário.
- Recalcular o rateio das viagens não faturadas quando houver alteração nas despesas fixas do mês.

## Regras de negócio
- "Veículos ativos" = veículos não inativados no mês de competência (independente do status operacional) (*Q4*).
- Dias da viagem contados de forma corrida e inclusiva (*Q3*); despesas fixas não são vinculadas a veículo (*Q8*).
- Viagens faturadas mantêm o rateio congelado no momento do faturamento (RNF03).
- Mês sem veículos ativos ou sem custos fixos → rateio zero.

## Critérios de aceite
- [ ] Custos fixos de R$ 30.000 com 5 veículos ativos resultam em R$ 200,00/dia por veículo.
- [ ] Uma viagem de 4 dias nesse mês recebe rateio de R$ 800,00.
- [ ] Uma viagem de 30/jan a 02/fev aplica a taxa de janeiro aos dias de janeiro e a de fevereiro aos de fevereiro.
- [ ] Alterar uma despesa fixa recalcula o rateio de viagens não faturadas do mês e não altera as faturadas.
