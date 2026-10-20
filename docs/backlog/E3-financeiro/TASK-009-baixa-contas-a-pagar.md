# TASK-009 — Baixa de títulos do Contas a Pagar

**Épico:** E3 — Orquestração Financeira · **Feature:** 3.1 · **Rastreabilidade:** US13 · **Depende de:** TASK-008

## Objetivo
Registrar o pagamento efetivo dos títulos, controlando o fluxo de caixa.

## Escopo
- Baixa manual informando a data real de pagamento (padrão: hoje) e forma de pagamento (opcional).
- Baixa em lote de múltiplos títulos selecionados com a mesma data (RNF01).
- Estorno de baixa (volta para Em aberto), com registro de quando foi estornado.

## Regras de negócio
- Data de pagamento obrigatória e não pode ser futura.
- No MVP a baixa é sempre do valor integral (sem pagamento parcial).
- Título cancelado não pode ser baixado.

## Critérios de aceite
- [ ] Ao dar baixa, o título passa para Pago com a data informada.
- [ ] A baixa em lote atualiza todos os títulos selecionados.
- [ ] O estorno retorna o título para Em aberto e remove a data de pagamento.
- [ ] Não é possível informar data de pagamento futura.
