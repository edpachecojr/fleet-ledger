# TASK-006 — Cadastro de clientes

**Épico:** E1 — Cadastros e Parâmetros Base · **Feature:** 1.2 · **Rastreabilidade:** RF03, US04 · **Depende de:** TASK-002

## Objetivo
Manter a base de clientes com os dados necessários para faturamento.

## Escopo
- Cadastro, edição, listagem (busca por razão social/CNPJ) e inativação.
- Campos: razão social, nome fantasia, CNPJ, contato (telefone/e-mail), ciclo de faturamento (Semanal, Quinzenal, Mensal), prazo de pagamento em dias (opcional, para sugerir vencimento da fatura).

## Regras de negócio
- CNPJ obrigatório, com dígitos verificadores válidos e único.
- Ciclo de faturamento obrigatório.
- Clientes com histórico não são excluídos, apenas inativados; inativos não aparecem na abertura de viagem.

## Critérios de aceite
- [ ] É possível cadastrar, editar e listar clientes.
- [ ] CNPJ inválido ou duplicado é rejeitado.
- [ ] O ciclo de faturamento é exibido na listagem e no detalhe.
- [ ] Cliente inativo não é selecionável em nova viagem.
