import { formatPaymentTerm, isBillingCycle, labelBillingCycle } from './customer'

describe('labelBillingCycle', () => {
  it.each([
    ['Weekly', 'Semanal'],
    ['Biweekly', 'Quinzenal'],
    ['Monthly', 'Mensal'],
  ] as const)('labels %s as "%s"', (cycle, label) => {
    expect(labelBillingCycle(cycle)).toBe(label)
  })
})

describe('isBillingCycle', () => {
  it('accepts only the API cycles', () => {
    expect(isBillingCycle('Monthly')).toBe(true)
    expect(isBillingCycle('Daily')).toBe(false)
  })
})

describe('formatPaymentTerm', () => {
  it('shows days, singular for one and a dash when not informed', () => {
    expect(formatPaymentTerm(30)).toBe('30 dias')
    expect(formatPaymentTerm(1)).toBe('1 dia')
    expect(formatPaymentTerm(null)).toBe('—')
  })
})
