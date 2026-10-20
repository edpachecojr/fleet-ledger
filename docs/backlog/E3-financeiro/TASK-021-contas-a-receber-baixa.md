# TASK-021 — Contas a Receber e baixa de faturas

**Épico:** E3 — Orquestração Financeira · **Feature:** 3.2 · **Rastreabilidade:** US16 · **Depende de:** TASK-020

## Objetivo
Acompanhar os recebimentos e registrar manualmente as faturas pagas pelos clientes.

## Escopo
- Listagem do Contas a Receber com filtros (cliente, vencimento, status: Em aberto, Recebido, Cancelado) e totalizadores.
- Destaque de títulos vencidos.
- Baixa manual informando a data real de recebimento (individual e em lote).
- Estorno da baixa.

## Regras de negócio
- Data de recebimento obrigatória e não futura.
- Baixa sempre pelo valor integral no MVP.

## Critérios de aceite
- [ ] A fatura gerada aparece no Contas a Receber como Em aberto.
- [ ] A baixa altera o status para Recebido com a data informada.
- [ ] O estorno retorna o título para Em aberto.
- [ ] Filtros e totalizadores retornam valores corretos.
