# TASK-011 — Abertura de viagem

**Épico:** E2 — Operação e Acerto de Viagens · **Feature:** 2.1 · **Rastreabilidade:** RF04, RF08, US06, US07 · **Depende de:** TASK-005, TASK-006, TASK-007

## Objetivo
Registrar o início de uma viagem em formato de bloco único, com os dados-base de faturamento e quilometragem.

## Escopo
- Formulário único de abertura: cliente, veículo, motorista, data/hora de saída, origem, destino, hodômetro inicial, valor bruto do frete, observações.
- Hodômetro inicial pré-preenchido com o hodômetro atual do veículo.
- Comissão do motorista copiada do cadastro no momento da abertura.
- Ao salvar: viagem com status **Aberta** e veículo com status **Em Viagem**.

## Regras de negócio
- Apenas cliente, veículo e motorista ativos; veículo deve estar Disponível (bloqueio de "Em Manutenção" e "Em Viagem").
- Um motorista não pode estar em duas viagens abertas simultaneamente.
- Hodômetro inicial não pode ser menor que o hodômetro atual do veículo.
- Valor do frete > 0.
- Aviso (sem bloqueio) para motorista com documentação vencida (*Q1*).

## Critérios de aceite
- [ ] Viagem aberta com sucesso altera o veículo para Em Viagem.
- [ ] Veículo Em Manutenção ou Em Viagem é rejeitado (UI e API).
- [ ] Motorista já em viagem aberta é rejeitado.
- [ ] Hodômetro inicial menor que o atual do veículo é rejeitado.
- [ ] A comissão gravada na viagem é a do cadastro no momento da abertura.
