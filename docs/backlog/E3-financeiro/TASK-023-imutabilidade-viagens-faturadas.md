# TASK-023 — Imutabilidade de viagens faturadas

**Épico:** E3 — Orquestração Financeira · **Rastreabilidade:** RNF03 · **Depende de:** TASK-020, TASK-013 a TASK-018

## Objetivo
Garantir que viagens faturadas não tenham dados financeiros nem de hodômetro alterados ou excluídos, preservando o histórico e a DRE.

## Escopo
- Bloqueio, na API e na interface, de edição/exclusão para viagens Faturadas: frete, hodômetros, datas, remuneração, adiantamentos, despesas, provisão, acerto, cancelamento.
- Interface em modo somente leitura para viagens Faturadas, com aviso explicativo.
- Único caminho de correção: cancelar a fatura (TASK-022), se permitido.

## Critérios de aceite
- [ ] Toda tentativa de alterar/excluir dados de viagem Faturada via API é rejeitada com mensagem clara.
- [ ] A tela da viagem Faturada não oferece ações de edição.
- [ ] Testes automatizados cobrem cada operação bloqueada.
