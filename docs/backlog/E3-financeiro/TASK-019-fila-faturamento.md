# TASK-019 — Fila de viagens aguardando faturamento

**Épico:** E3 — Orquestração Financeira · **Feature:** 3.2 · **Rastreabilidade:** US14 · **Depende de:** TASK-017

## Objetivo
Dar visibilidade às viagens fechadas que ainda precisam ser cobradas, organizadas por cliente.

## Escopo
- Lista de viagens Fechadas e não faturadas, agrupadas por cliente, com: data de fechamento, rota, veículo, valor do frete e total por cliente.
- Indicação do ciclo de faturamento do cliente e de clientes cujo ciclo já venceu (ex.: ciclo semanal com viagens fechadas há mais de 7 dias).
- Filtros por cliente e período; seleção de viagens para iniciar uma fatura (TASK-020).

## Critérios de aceite
- [ ] Toda viagem Fechada e não faturada aparece na fila; viagens Abertas, Canceladas ou Faturadas não aparecem.
- [ ] O total por cliente corresponde à soma dos fretes listados.
- [ ] Clientes com ciclo vencido são destacados.
