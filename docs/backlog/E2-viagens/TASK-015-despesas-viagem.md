# TASK-015 — Lançamento de despesas da viagem

**Épico:** E2 — Operação e Acerto de Viagens · **Feature:** 2.2 · **Rastreabilidade:** US09, RF12, RNF01 · **Depende de:** TASK-011, TASK-008, TASK-003

## Objetivo
Registrar os custos variáveis da rota (combustível, pedágio, chapas etc.) classificados no plano de contas.

## Escopo
- Lançamento rápido em grade, com várias despesas de uma vez (teclado, mínimo de cliques): data, conta (grupo Despesas variáveis), descrição, valor, quem pagou (empresa ou motorista).
- Para combustível: litros e km (opcionais) para indicadores futuros.
- Despesa paga pela empresa → título no Contas a Pagar (por padrão já baixado).
- Despesa paga pelo motorista (com o adiantamento ou recursos próprios) → valor somado no acerto final (sem título separado), pois o adiantamento já foi deduzido lá.
- Total de despesas por conta no detalhe da viagem.

## Regras de negócio
- Regra de "quem pagou" conforme *Q6*.
- Apenas contas do grupo "Despesas variáveis de viagem".
- Despesas podem ser lançadas em viagens Abertas ou Fechadas não faturadas.

## Critérios de aceite
- [ ] É possível lançar múltiplas despesas em sequência sem sair da tela.
- [ ] Cada despesa respeita a regra de origem do pagamento (empresa → título; motorista → acerto).
- [ ] O total de despesas da viagem é exibido por conta e no geral.
- [ ] Contas fora do grupo de despesas variáveis são rejeitadas.
