# TASK-010 — Lançamento de despesas fixas da frota

**Épico:** E3 — Orquestração Financeira · **Feature:** 3.1 · **Rastreabilidade:** US12, RF13 · **Depende de:** TASK-008

## Objetivo
Registrar os custos fixos mensais (seguro, IPVA, aluguel etc.) que alimentam o rateio de custo fixo.

## Escopo
- Lançamento de despesa fixa: conta (grupo Custos fixos), descrição, valor, mês de competência, vencimento.
- Opção de recorrência mensal (gerar automaticamente para os próximos N meses) para agilizar a digitação.
- Opção de parcelamento de despesas anuais (ex.: IPVA, seguro) distribuindo a competência em meses.
- Cada despesa fixa gera um título no Contas a Pagar.
- Consulta do total de custos fixos por mês de competência.

## Regras de negócio
- O rateio utiliza o **mês de competência**, não a data de pagamento.
- Despesas fixas são da empresa como um todo (não vinculadas a um veículo específico) no MVP.
- Editar/excluir uma despesa fixa em aberto atualiza/cancela o título correspondente; despesa já paga não pode ser excluída.

## Critérios de aceite
- [ ] Ao lançar uma despesa fixa, um título correspondente aparece no Contas a Pagar.
- [ ] A recorrência gera um lançamento por mês com a competência correta.
- [ ] O total de custos fixos de um mês reflete exatamente as despesas daquela competência.
- [ ] Contas fora do grupo Custos fixos não são selecionáveis.
