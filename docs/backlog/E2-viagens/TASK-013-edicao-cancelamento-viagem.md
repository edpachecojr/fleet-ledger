# TASK-013 — Edição e cancelamento de viagem aberta

**Épico:** E2 — Operação e Acerto de Viagens · **Rastreabilidade:** RF04, RNF03 · **Depende de:** TASK-011

## Objetivo
Permitir corrigir erros de digitação e cancelar viagens que não ocorreram.

## Escopo
- Edição dos dados de abertura enquanto a viagem estiver Aberta.
- Troca de veículo/motorista na viagem aberta (revalidando elegibilidade e liberando o veículo anterior).
- Cancelamento de viagem aberta com motivo.

## Regras de negócio
- Cancelamento só é permitido se não houver adiantamentos pagos; adiantamentos/despesas em aberto são cancelados junto (com confirmação).
- Ao cancelar, o veículo volta para Disponível e o hodômetro não é alterado.
- Viagens Fechadas não são editáveis nesta tarefa e não podem ser reabertas no MVP (*Q2*); viagens Faturadas são imutáveis (TASK-023).

## Critérios de aceite
- [ ] Alterações em viagem aberta são salvas e refletidas no detalhe.
- [ ] Trocar o veículo libera o anterior (Disponível) e ocupa o novo (Em Viagem).
- [ ] Cancelar uma viagem libera o veículo e cancela os títulos em aberto vinculados.
- [ ] Viagem com adiantamento pago não pode ser cancelada.
