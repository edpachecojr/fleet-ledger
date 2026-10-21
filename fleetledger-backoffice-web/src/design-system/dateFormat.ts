/**
 * Data sem hora da API ("2026-11-10") em pt-BR, sem passar por Date: evita o deslocamento de
 * fuso que transformaria o dia 10 em 9 no Brasil.
 * @example formatDateOnly('2026-11-10') // '10/11/2026'
 */
export function formatDateOnly(isoDate: string | null | undefined): string {
  const match = /^(\d{4})-(\d{2})-(\d{2})$/.exec(isoDate ?? '')
  if (!match) return '—'
  const [, year, month, day] = match
  return `${day ?? ''}/${month ?? ''}/${year ?? ''}`
}
