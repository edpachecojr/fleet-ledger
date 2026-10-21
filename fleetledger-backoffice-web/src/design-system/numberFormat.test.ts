import { formatCurrency, formatPercent, formatQuantity, parseLocaleNumber } from './numberFormat'

// Intl separa "R$" do valor com espaço não separável.
const normalizeSpaces = (text: string): string => text.replace(/\s/g, ' ')

describe('formatCurrency', () => {
  it('formats reais with cents and thousands separator', () => {
    expect(normalizeSpaces(formatCurrency(1249))).toBe('R$ 1.249,00')
  })
})

describe('formatQuantity', () => {
  it('formats integers with thousands separator and the unit', () => {
    expect(formatQuantity(27000, 'kg')).toBe('27.000 kg')
  })
})

describe('parseLocaleNumber', () => {
  it.each([
    ['0,15', 0.15],
    ['1.249,50', 1249.5],
    ['27.000', 27000],
    ['27000', 27000],
    ['0.15', 0.15],
    [' 12 ', 12],
    ['-3', -3],
  ])('reads "%s" as %s', (input, expected) => {
    expect(parseLocaleNumber(input)).toBe(expected)
  })

  it.each(['', 'abc', '1,2,3', '12a'])('returns null for "%s"', (input) => {
    expect(parseLocaleNumber(input)).toBeNull()
  })
})

describe('formatPercent', () => {
  it('formats with two decimals and a comma', () => {
    expect(formatPercent(12.5)).toBe('12,50%')
  })
})
