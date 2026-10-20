# TASK-031 — Dashboard inicial: indicadores operacionais

**Épico:** E5 — Resultados Econômicos · **Rastreabilidade:** US18, RNF04, RF02, RF11 · **Depende de:** TASK-026, TASK-012, TASK-007

## Objetivo
Exibir, na tela inicial, os pontos de atenção operacionais da frota de forma imediata.

## Escopo
- Cartões/listas na tela inicial:
  - Viagens em andamento (veículo, motorista, cliente, dias em viagem).
  - Alertas de manutenção Amarelo e Vermelho por caminhão (com atalho para o registro da manutenção).
  - Status da frota (quantidade Disponível / Em Viagem / Em Manutenção).
  - Motoristas com CNH ou toxicológico vencidos ou vencendo em 30 dias.
  - Viagens fechadas aguardando faturamento (quantidade e valor).
- Tela inicial padrão após o login.

## Regras de negócio
- Carregamento instantâneo (RNF04): meta de resposta definida pelo time (ex.: < 1 s com volume de dados de referência).

## Critérios de aceite
- [ ] Todos os indicadores refletem o estado atual dos dados.
- [ ] Clicar em um item leva à tela correspondente (viagem, veículo, motorista).
- [ ] O dashboard atende à meta de desempenho com a massa de dados de referência.
