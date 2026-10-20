# TASK-027 — Registro de manutenção executada

**Épico:** E4 — Controle de Manutenção · **Feature:** 4.1 · **Rastreabilidade:** US19, US21 · **Depende de:** TASK-025, TASK-008

## Objetivo
Registrar manutenções realizadas, zerando os contadores dos itens do plano e lançando o custo real no Contas a Pagar.

## Escopo
- Registro de manutenção: veículo, data, hodômetro na execução, oficina/fornecedor, valor, vencimento do pagamento, descrição, itens do plano atendidos (zero ou mais — permite manutenção corretiva/avulsa).
- Para cada item do plano atendido: hodômetro da última execução = hodômetro informado (contador zerado).
- Geração de título no Contas a Pagar (conta "Manutenção realizada"), vinculado ao veículo.
- Histórico de manutenções por veículo.
- Opção de retornar o veículo para Disponível ao registrar, se estiver Em Manutenção.

## Regras de negócio
- Valor > 0; hodômetro informado ≤ hodômetro atual do veículo e ≥ última execução do item.
- O custo real compõe a DRE mensal do veículo (comparado à provisão), mas **não** entra na DRE da viagem.
- Excluir um registro só é possível se o título estiver em aberto; os contadores voltam ao estado anterior.

## Critérios de aceite
- [ ] Registrar "Troca de óleo" zera o contador do item e o semáforo volta a Verde.
- [ ] Um título com o valor da manutenção aparece no Contas a Pagar vinculado ao veículo.
- [ ] Manutenção sem item de plano (corretiva) é registrada e gera título normalmente.
- [ ] O histórico do veículo exibe a manutenção registrada.
