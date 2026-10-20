# TASK-026 — Semáforo de alertas de manutenção

**Épico:** E4 — Controle de Manutenção · **Feature:** 4.1 · **Rastreabilidade:** RF11, US18 · **Depende de:** TASK-025, TASK-017

## Objetivo
Classificar automaticamente cada item de manutenção em Verde / Amarelo / Vermelho conforme o desgaste.

## Escopo
- Cálculo do percentual consumido: (hodômetro atual − última execução) / intervalo.
- Classificação: **Verde** < 90%; **Amarelo** ≥ 90% e < 100%; **Vermelho** ≥ 100%.
- Recálculo sempre que o hodômetro do veículo mudar (fechamento de viagem) ou o plano/execução for alterado.
- Painel de manutenção da frota: todos os itens em alerta, ordenados por criticidade, com filtro por veículo e cor.
- Indicador resumido no cadastro/listagem de veículos (pior status do veículo).

## Critérios de aceite
- [ ] Intervalo 20.000 km: 17.999 km rodados = Verde; 18.000 = Amarelo; 20.000 = Vermelho.
- [ ] Fechar uma viagem que leva o item além de 90% o torna Amarelo imediatamente.
- [ ] O painel lista apenas itens Amarelo/Vermelho por padrão, ordenados do mais crítico.
