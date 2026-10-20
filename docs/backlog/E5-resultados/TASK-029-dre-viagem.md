# TASK-029 — DRE da viagem

**Épico:** E5 — Resultados Econômicos · **Feature:** 5.1 · **Rastreabilidade:** RF12, US20 · **Depende de:** TASK-015, TASK-016, TASK-017, TASK-020, TASK-028

## Objetivo
Exibir o resultado econômico de uma viagem específica para conhecer a margem exata da rota.

## Escopo
- Demonstrativo na tela da viagem:
  - (+) Frete bruto
  - (−/+) Descontos e acréscimos rateados da fatura
  - (=) Receita líquida
  - (−) Despesas variáveis (detalhadas por conta)
  - (−) Remuneração do motorista
  - (−) Provisão de manutenção
  - (=) Margem de contribuição
  - (−) Rateio de custo fixo
  - (=) **Resultado da viagem** e margem % sobre a receita líquida
- Indicadores complementares: km rodados, receita por km, custo por km.
- Disponível para viagens Fechadas e Faturadas (prévia para Abertas, com aviso de valores parciais).
- Exportação/impressão do demonstrativo.

## Regras de negócio
- Adiantamentos não entram na DRE (são movimentação de caixa).
- Custo real de oficina não entra na DRE da viagem (é coberto pela provisão).

## Critérios de aceite
- [ ] Frete R$ 10.000, despesas R$ 3.000, remuneração R$ 1.200, provisão R$ 450 e rateio R$ 800 resultam em resultado R$ 4.550 e margem 45,5%.
- [ ] Os valores batem com os lançamentos da viagem.
- [ ] O desconto rateado da fatura reduz a receita líquida da viagem.
