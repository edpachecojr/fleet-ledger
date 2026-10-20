# TASK-020 — Geração de fatura

**Épico:** E3 — Orquestração Financeira · **Feature:** 3.2 · **Rastreabilidade:** RF09, US15 · **Depende de:** TASK-019

## Objetivo
Consolidar várias viagens de um mesmo cliente em uma única fatura e lançá-la no Contas a Receber.

## Escopo
- Seleção de viagens do mesmo cliente a partir da fila.
- Pré-visualização da fatura: viagens, subtotal de fretes, descontos manuais, acréscimos manuais (cada um com descrição), total líquido.
- Data de emissão e vencimento (sugerido pelo prazo de pagamento do cliente, editável).
- Confirmação: número sequencial da fatura, título no Contas a Receber e viagens passam para status **Faturada**.
- Visualização/impressão da fatura (espelho para envio ao cliente).

## Regras de negócio
- Todas as viagens precisam ser do mesmo cliente e estar Fechadas e não faturadas.
- Total líquido > 0.
- Descontos/acréscimos são rateados entre as viagens proporcionalmente ao frete, para refletir na DRE de cada viagem.

## Critérios de aceite
- [ ] Faturar 3 viagens (R$ 5.000 + R$ 3.000 + R$ 2.000) com desconto de R$ 500 gera título de R$ 9.500.
- [ ] O rateio do desconto atribui R$ 250 / R$ 150 / R$ 100 às viagens.
- [ ] Viagens de clientes diferentes não podem compor a mesma fatura.
- [ ] Após a confirmação, as viagens saem da fila e ficam com status Faturada.
