# TASK-004 — Cadastro de veículos

**Épico:** E1 — Cadastros e Parâmetros Base · **Feature:** 1.1 · **Rastreabilidade:** RF01, US01, US02 · **Depende de:** TASK-002

## Objetivo
Manter o inventário da frota com os dados necessários para operação e cálculo de provisão de manutenção.

## Escopo
- Cadastro, edição, listagem (com busca por placa/modelo) e inativação de veículos.
- Campos: placa, modelo, capacidade, hodômetro inicial, taxa de provisão de manutenção (R$/km).
- Exibição do hodômetro atual (inicialmente igual ao hodômetro inicial).
- Status operacional inicial "Disponível".

## Regras de negócio
- Placa obrigatória, em formato válido (antigo ou Mercosul) e única.
- Hodômetro inicial ≥ 0; taxa de provisão ≥ 0.
- Após o cadastro, o hodômetro atual só é alterado pelo sistema (fechamento de viagem / registro de manutenção), nunca por edição livre.
- Veículos com histórico não são excluídos, apenas inativados. Veículo inativo não pode ser alocado e não entra no rateio de custo fixo.
- Alteração da taxa de provisão vale apenas para viagens fechadas após a alteração.

## Critérios de aceite
- [ ] É possível cadastrar um veículo com todos os campos e vê-lo na listagem.
- [ ] Placa duplicada ou inválida é rejeitada com mensagem clara.
- [ ] A edição permite alterar modelo, capacidade e taxa de provisão, mas não o hodômetro atual.
- [ ] Inativar um veículo o remove das seleções de nova viagem.
- [ ] A busca por placa/modelo retorna os veículos correspondentes.
