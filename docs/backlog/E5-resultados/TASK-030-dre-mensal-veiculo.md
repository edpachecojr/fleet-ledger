# TASK-030 — DRE mensal por veículo

**Épico:** E5 — Resultados Econômicos · **Feature:** 5.1 · **Rastreabilidade:** US21, RF12 · **Depende de:** TASK-029, TASK-027

## Objetivo
Consolidar o resultado mensal de cada caminhão, comparando a provisão de manutenção com o gasto real em oficina.

## Escopo
- Seleção de mês e veículo (ou todos os veículos lado a lado).
- Consolidação das viagens do mês (critério de competência: data de fechamento da viagem, *Q7*): receita líquida, despesas variáveis, remuneração, provisão, rateio fixo.
- Bloco comparativo de manutenção: provisão gerada no mês × manutenção efetivamente gasta no mês (títulos de manutenção do veículo) e diferença.
- Dois resultados: **Resultado econômico** (com provisão) e **Resultado real** (substituindo a provisão pelo gasto real).
- Indicadores: km rodados no mês, nº de viagens, receita/km, custo/km.
- Saldo acumulado de provisão do veículo (provisão total − gasto real total desde o início).

## Critérios de aceite
- [ ] A soma das DREs das viagens do mês do veículo corresponde ao resultado econômico exibido.
- [ ] O gasto real considera apenas manutenções do veículo com competência no mês.
- [ ] A diferença provisão × real é exibida corretamente.
- [ ] A visão "todos os veículos" permite comparar os caminhões no mês.
