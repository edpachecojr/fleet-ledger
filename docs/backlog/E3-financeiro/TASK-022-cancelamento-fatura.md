# TASK-022 — Cancelamento de fatura

**Épico:** E3 — Orquestração Financeira · **Feature:** 3.2 · **Rastreabilidade:** RF09, RNF03 · **Depende de:** TASK-021

## Objetivo
Permitir corrigir uma fatura emitida com erro, devolvendo as viagens à fila de faturamento de forma controlada.

## Escopo
- Cancelamento de fatura com motivo obrigatório.
- Cancelamento do título correspondente no Contas a Receber.
- Viagens voltam para status Fechada e retornam à fila de faturamento; o rateio de desconto/acréscimo é removido.
- Histórico do cancelamento preservado (fatura permanece consultável como Cancelada).

## Regras de negócio
- Só é possível cancelar fatura cujo título **não** esteja recebido (estornar a baixa antes).
- O número da fatura cancelada não é reutilizado.

## Critérios de aceite
- [ ] Cancelar uma fatura em aberto devolve suas viagens à fila.
- [ ] Fatura com título recebido não pode ser cancelada.
- [ ] A fatura cancelada continua visível com motivo e data.
