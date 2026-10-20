# TASK-016 — Cálculo da remuneração do motorista

**Épico:** E2 — Operação e Acerto de Viagens · **Rastreabilidade:** RF06 · **Depende de:** TASK-011

## Objetivo
Calcular automaticamente a remuneração do motorista na viagem, com possibilidade de ajuste manual.

## Escopo
- Cálculo: remuneração = valor bruto do frete × comissão (%) gravada na viagem.
- Recalcular automaticamente quando o frete ou a comissão da viagem forem alterados (enquanto não houver ajuste manual).
- Edição manual do valor final com justificativa obrigatória; opção de voltar ao valor calculado.
- Exibição lado a lado de valor calculado e valor final no detalhe da viagem.

## Regras de negócio
- Valor final ≥ 0.
- Remuneração é despesa da viagem na DRE.
- Editável até o acerto final; após o acerto, imutável.

## Critérios de aceite
- [ ] Frete R$ 10.000 com comissão 12% resulta em remuneração calculada de R$ 1.200.
- [ ] Alterar o frete recalcula a remuneração quando não há ajuste manual.
- [ ] Um ajuste manual prevalece e registra a justificativa.
- [ ] Após o acerto, a remuneração não pode ser alterada.
