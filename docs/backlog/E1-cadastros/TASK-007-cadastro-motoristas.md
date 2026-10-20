# TASK-007 — Cadastro de motoristas

**Épico:** E1 — Cadastros e Parâmetros Base · **Feature:** 1.2 · **Rastreabilidade:** RF02, US05 · **Depende de:** TASK-002

## Objetivo
Manter o cadastro de motoristas com comissão padrão e controle de vencimento de documentos.

## Escopo
- Cadastro, edição, listagem (busca por nome/CPF) e inativação.
- Campos: nome, CPF, telefone, número e categoria da CNH, validade da CNH, validade do exame toxicológico, comissão padrão (%), dados bancários/PIX (opcional, para pagamento do acerto).
- Indicador de situação documental na listagem: Regular, Vencendo (próximos 30 dias), Vencido.

## Regras de negócio
- CPF válido e único; comissão entre 0% e 100%.
- Alteração da comissão padrão não afeta viagens já abertas.
- Motoristas inativos não aparecem na abertura de viagem.
- Documento vencido gera alerta na abertura de viagem (bloqueio ou apenas aviso: **a decidir**).

## Critérios de aceite
- [ ] É possível cadastrar, editar e listar motoristas.
- [ ] CPF inválido/duplicado ou comissão fora da faixa é rejeitada.
- [ ] A listagem exibe corretamente a situação documental conforme as datas.
- [ ] É possível filtrar motoristas com documentos vencidos ou vencendo.
