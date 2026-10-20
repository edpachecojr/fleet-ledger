# TASK-018 — Acerto final com o motorista

**Épico:** E2 — Operação e Acerto de Viagens · **Feature:** 2.2 · **Rastreabilidade:** US11, RF05, RF06, RNF01 · **Depende de:** TASK-014, TASK-015, TASK-016, TASK-017

## Objetivo
Apurar o valor exato devido ao motorista e gerar a conta a pagar correspondente.

## Escopo
- Tela de acerto (resumo em uma única tela) com:
  - Remuneração final (+)
  - Despesas da viagem pagas pelo motorista (+)
  - Créditos pendentes de acertos anteriores (−)
  - Adiantamentos recebidos (−)
  - **Saldo do acerto**
- Confirmação do acerto gera título no Contas a Pagar ao motorista (saldo positivo) com vencimento informado.
- Saldo negativo (motorista deve à empresa): gera crédito da empresa, abatido automaticamente no próximo acerto do motorista (*Q5*).
- Comprovante/resumo do acerto imprimível.

## Regras de negócio
- O acerto só pode ser feito com a viagem Fechada.
- Após confirmado, remuneração, adiantamentos e despesas pagas pelo motorista ficam bloqueados.
- Desfazer o acerto só é permitido se o título do acerto ainda não foi pago.

## Critérios de aceite
- [ ] Remuneração R$ 1.200, despesas pagas pelo motorista R$ 150 e adiantamento R$ 800 resultam em saldo R$ 550 e título de R$ 550.
- [ ] Após o acerto, os itens que o compõem não podem ser alterados.
- [ ] Desfazer o acerto cancela o título em aberto e libera a edição.
- [ ] Viagem Aberta não permite acerto.
- [ ] Acerto com saldo negativo gera crédito que é abatido no acerto seguinte do mesmo motorista.
