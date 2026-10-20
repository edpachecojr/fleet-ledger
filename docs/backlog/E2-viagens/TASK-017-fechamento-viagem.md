# TASK-017 — Fechamento da viagem

**Épico:** E2 — Operação e Acerto de Viagens · **Feature:** 2.2 · **Rastreabilidade:** RF07, US10 · **Depende de:** TASK-011, TASK-004

## Objetivo
Encerrar a viagem registrando o hodômetro final, atualizando o veículo e gerando a provisão de manutenção.

## Escopo
- Ação de fechamento: hodômetro final e data/hora de chegada.
- Cálculo da distância percorrida (final − inicial) e da duração em dias.
- Atualização do hodômetro atual do veículo e retorno do status para Disponível.
- Geração da despesa **virtual** de Provisão de Manutenção = distância × taxa R$/km do veículo vigente no fechamento.
- Disparo do recálculo dos alertas de manutenção do veículo (integração com TASK-026, quando disponível).
- Viagem passa para status **Fechada** e entra na fila de faturamento.

## Regras de negócio
- Hodômetro final > hodômetro inicial; data de chegada ≥ data de saída.
- A provisão é virtual: aparece na DRE, mas **não** gera título no Contas a Pagar nem movimenta caixa.
- Dias da viagem (para rateio) = dias corridos, inclusivos: o dia de saída e o de chegada contam (*Q3*).
- Após fechada, a viagem não pode ser reaberta (*Q2*).

## Critérios de aceite
- [ ] Hodômetro 100.000 → 101.500 com taxa R$ 0,30/km gera distância 1.500 km e provisão R$ 450,00.
- [ ] O hodômetro atual do veículo passa a ser o hodômetro final.
- [ ] O veículo volta para Disponível.
- [ ] Hodômetro final menor ou igual ao inicial é rejeitado.
- [ ] Nenhum título é criado no Contas a Pagar para a provisão.
