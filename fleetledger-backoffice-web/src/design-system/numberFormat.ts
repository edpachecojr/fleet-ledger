const LOCALE = 'pt-BR'

const CURRENCY = new Intl.NumberFormat(LOCALE, { style: 'currency', currency: 'BRL' })
const INTEGER = new Intl.NumberFormat(LOCALE, { maximumFractionDigits: 0 })

/**
 * Moeda do design system.
 * @example formatCurrency(1249) // 'R$ 1.249,00'
 */
export function formatCurrency(value: number): string {
  return CURRENCY.format(value)
}

/**
 * Inteiro com separador de milhar e unidade.
 * @example formatQuantity(27000, 'kg') // '27.000 kg'
 */
export function formatQuantity(value: number, unit: string): string {
  return `${INTEGER.format(value)} ${unit}`
}

// Só pontos em grupos de 3 ("27.000") são milhar; senão o ponto é decimal ("0.15").
const THOUSANDS_ONLY = /^-?\d{1,3}(\.\d{3})+$/
const PLAIN_NUMBER = /^-?\d+(\.\d+)?$/

/**
 * Lê número digitado em pt-BR ("1.249,50", "27.000" ou "0,15"); aceita ponto decimal
 * ("0.15") por engano comum. `null` quando não é número.
 * @example parseLocaleNumber('0,15') // 0.15
 */
export function parseLocaleNumber(input: string): number | null {
  const normalized = normalizeDecimalSeparators(input.trim())
  return PLAIN_NUMBER.test(normalized) ? Number(normalized) : null
}

function normalizeDecimalSeparators(input: string): string {
  if (input.includes(',')) return input.replaceAll('.', '').replace(',', '.')
  return THOUSANDS_ONLY.test(input) ? input.replaceAll('.', '') : input
}

const PERCENT = new Intl.NumberFormat('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })

/**
 * Percentual com 2 casas.
 * @example formatPercent(12.5) // '12,50%'
 */
export function formatPercent(value: number): string {
  return `${PERCENT.format(value)}%`
}
