# TASK-025 — Planos de manutenção por veículo

**Épico:** E4 — Controle de Manutenção · **Feature:** 4.1 · **Rastreabilidade:** RF11, US17 · **Depende de:** TASK-004

## Objetivo
Definir, para cada veículo, os itens de manutenção preventiva e seus intervalos em km, que servirão de gatilho para os alertas.

## Escopo
- Cadastro, edição e desativação de itens de plano por veículo: descrição (ex.: Troca de óleo), intervalo em km, hodômetro da última execução (padrão: hodômetro atual do veículo).
- Opção de copiar os planos de um veículo para outro (agiliza a configuração de frotas iguais).
- Exibição, por item: km rodados desde a última execução, km restantes e próximo hodômetro previsto.

## Regras de negócio
- Intervalo em km > 0.
- Hodômetro da última execução ≤ hodômetro atual do veículo.

## Critérios de aceite
- [ ] É possível cadastrar "Troca de óleo a cada 20.000 km" para um veículo.
- [ ] Com última execução em 100.000 e hodômetro atual 112.000, o item mostra 12.000 km rodados e 8.000 km restantes.
- [ ] Copiar planos replica os itens para o veículo de destino.
