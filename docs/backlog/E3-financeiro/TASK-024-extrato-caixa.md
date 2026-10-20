# TASK-024 — Extrato de caixa

**Épico:** E3 — Orquestração Financeira · **Rastreabilidade:** US13, US16 · **Depende de:** TASK-009, TASK-021

## Objetivo
Mostrar o fluxo de caixa realizado da empresa a partir das baixas de pagamentos e recebimentos.

## Escopo
- Saldo inicial configurável (uma única vez, na implantação).
- Extrato por período: entradas (recebimentos) e saídas (pagamentos) por data real, com saldo acumulado.
- Visão de caixa previsto: títulos em aberto a pagar e a receber por vencimento nos próximos dias.
- Filtro por conta do plano de contas.

## Regras de negócio
- Provisão de manutenção (virtual) nunca aparece no caixa.
- Estornos de baixa removem o movimento do extrato.

## Critérios de aceite
- [ ] Saldo final = saldo inicial + recebimentos − pagamentos do período.
- [ ] Apenas títulos baixados aparecem no extrato realizado.
- [ ] O caixa previsto lista os títulos em aberto por vencimento.
