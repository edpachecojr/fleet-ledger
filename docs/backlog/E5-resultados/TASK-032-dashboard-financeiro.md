# TASK-032 — Dashboard inicial: resultado financeiro do mês

**Épico:** E5 — Resultados Econômicos · **Rastreabilidade:** US22, RNF04 · **Depende de:** TASK-030, TASK-024, TASK-031

## Objetivo
Resumir na tela inicial o lucro da transportadora no mês atual para decisões rápidas.

## Escopo
- Cartões do mês atual: receita líquida, custos variáveis, remuneração, provisão de manutenção, custos fixos totais e **lucro total da transportadora**.
- Comparativo com o mês anterior (variação %).
- Ranking de resultado por veículo no mês.
- Contas a pagar e a receber vencidas e a vencer nos próximos 7 dias.
- Saldo de caixa atual.

## Regras de negócio
- Lucro total = soma dos resultados econômicos dos veículos no mês − custos fixos não absorvidos pelo rateio (se houver).
- Carregamento instantâneo (RNF04).

## Critérios de aceite
- [ ] O lucro total do mês confere com a soma das DREs mensais por veículo e os custos fixos do mês.
- [ ] Os valores de contas vencidas/a vencer conferem com os módulos financeiros.
- [ ] O dashboard atende à meta de desempenho definida.
