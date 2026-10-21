# TASK-003 — Plano de Contas padrão

**Épico:** E1 — Cadastros e Parâmetros Base · **Rastreabilidade:** RF10, US09 · **Depende de:** TASK-001, TASK-002

## Objetivo
Disponibilizar um Plano de Contas fixo e inflexível, base de classificação de todos os lançamentos e da DRE.

## Escopo
- Carga inicial (seed) do plano de contas com grupos e contas analíticas. Proposta inicial:
  - **Receitas:** Frete.
  - **Deduções/Ajustes de receita:** Descontos concedidos, Acréscimos cobrados.
  - **Despesas variáveis de viagem:** Combustível, Pedágio, Chapas/Descarga, Alimentação/Diárias, Outras despesas de viagem.
  - **Remuneração de motoristas:** Comissão/Remuneração de viagem.
  - **Manutenção:** Provisão de manutenção (virtual), Manutenção realizada (oficina/peças).
  - **Custos fixos:** Seguro, IPVA/Licenciamento, Aluguel, Rastreamento, Salários fixos, Outros custos fixos.
  - **Movimentações não-resultado:** Adiantamento a motorista.
- Consulta (somente leitura) do plano de contas na interface.
- Identificação de quais contas aceitam lançamento manual e em qual contexto (viagem, despesa fixa, manutenção).

## Regras de negócio
- O administrador **não** pode criar, editar ou excluir contas.
- Cada conta pertence a um grupo da DRE, determinando onde ela aparece nos demonstrativos.
- Contas de uso sistêmico (ex.: Provisão de manutenção, Adiantamento) não aparecem em seleções de lançamento manual.

## Critérios de aceite
- [x] Após a instalação, o plano de contas está disponível sem ação do usuário.
- [x] A tela de consulta lista grupos e contas em hierarquia.
- [x] Não há operação disponível (UI ou API) para alterar o plano de contas.
- [x] As listas de seleção de contas filtram corretamente por contexto de uso.

## Observações
Lista de contas acima aprovada pelo gestor (*Q9*).
