# TASK-005 — Status operacional do veículo e bloqueio de alocação

**Épico:** E1 — Cadastros e Parâmetros Base · **Feature:** 1.1 · **Rastreabilidade:** RF01, RF08, US03 · **Depende de:** TASK-004

## Objetivo
Controlar o status operacional (Disponível, Em Viagem, Em Manutenção) e impedir alocação indevida.

## Escopo
- Ação manual para colocar o veículo "Em Manutenção" e para retorná-lo a "Disponível".
- Exibição do status na listagem e no detalhe do veículo (com destaque visual).
- Regra de elegibilidade reutilizável: "veículo pode ser alocado em viagem?".

## Regras de negócio
- "Em Viagem" é definido apenas pelo sistema (abertura/fechamento/cancelamento de viagem), não manualmente.
- Não é permitido colocar "Em Manutenção" um veículo que está "Em Viagem".
- Somente veículos ativos e "Disponível" são elegíveis para alocação.

## Critérios de aceite
- [x] O administrador altera um veículo Disponível para Em Manutenção e vice-versa.
- [x] A tentativa de alterar manualmente para/desde "Em Viagem" é bloqueada.
- [x] Um veículo Em Manutenção não aparece/é rejeitado na abertura de viagem (validado também na API).
- [x] A listagem permite filtrar por status.
