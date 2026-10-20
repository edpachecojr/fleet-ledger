# TASK-014 — Adiantamentos ao motorista

**Épico:** E2 — Operação e Acerto de Viagens · **Feature:** 2.1 · **Rastreabilidade:** RF05, US08 · **Depende de:** TASK-011, TASK-008

## Objetivo
Registrar valores adiantados ao motorista durante a viagem, refletindo no caixa e deduzindo no acerto final.

## Escopo
- Lançamento de um ou mais adiantamentos na viagem: valor, data, forma de pagamento, observação.
- Cada adiantamento gera um título no Contas a Pagar (conta "Adiantamento a motorista"), por padrão já baixado na data informada (opção de deixar em aberto).
- Lista e total de adiantamentos no detalhe da viagem; edição/exclusão enquanto a viagem estiver Aberta.

## Regras de negócio
- Permitido apenas para viagens Abertas (ou Fechadas ainda não acertadas).
- Adiantamento é movimentação de caixa, **não** é despesa na DRE (é deduzido da remuneração no acerto).
- Excluir um adiantamento cancela/estorna o título correspondente.

## Critérios de aceite
- [ ] Ao lançar um adiantamento, o título aparece no Contas a Pagar vinculado à viagem.
- [ ] O total de adiantamentos da viagem é exibido corretamente.
- [ ] Adiantamentos não aparecem como despesa na DRE da viagem.
- [ ] Após o acerto, não é possível lançar novos adiantamentos.
