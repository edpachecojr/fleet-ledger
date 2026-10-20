# TASK-008 — Contas a Pagar: títulos e consulta

**Épico:** E3 — Orquestração Financeira · **Feature:** 3.1 · **Rastreabilidade:** RF05, US13 · **Depende de:** TASK-003

## Objetivo
Criar o núcleo do Contas a Pagar, que recebe títulos gerados automaticamente pelos módulos (adiantamentos, despesas, acertos, manutenções, despesas fixas) e lançamentos avulsos.

## Escopo
- Estrutura de título a pagar: descrição, favorecido, conta do plano de contas, valor, data de competência, data de vencimento, status (Em aberto, Pago, Cancelado), origem (Manual, Adiantamento, Despesa de viagem, Acerto, Manutenção, Despesa fixa) e vínculo com viagem/veículo quando houver.
- Lançamento manual de título avulso.
- Listagem com filtros por período de vencimento, status, origem, conta, veículo e viagem; totalizadores do filtro.
- Destaque para títulos vencidos.
- Serviço interno reutilizável para os demais módulos gerarem títulos.

## Regras de negócio
- Valor > 0; conta do plano de contas obrigatória.
- Títulos gerados por outros módulos só podem ser alterados/cancelados pelo módulo de origem.
- Títulos avulsos em aberto podem ser editados ou cancelados; títulos pagos não.

## Critérios de aceite
- [ ] O administrador lança um título avulso e o vê na listagem com status Em aberto.
- [ ] Filtros e totalizadores retornam valores corretos.
- [ ] Títulos vencidos e não pagos aparecem destacados.
- [ ] Um título de origem automática não pode ser editado diretamente na tela do Contas a Pagar.
