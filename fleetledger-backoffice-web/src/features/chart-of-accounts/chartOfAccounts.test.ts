import { labelEntryContext, titleOfGroup } from './chartOfAccounts'

describe('labelEntryContext', () => {
  it.each([
    ['Trip', 'Viagem'],
    ['FixedExpense', 'Despesa fixa'],
    ['Maintenance', 'Manutenção'],
    ['Billing', 'Faturamento'],
  ] as const)('labels %s as "%s"', (entryContext, label) => {
    expect(labelEntryContext(entryContext)).toBe(label)
  })

  it('returns null for system accounts', () => {
    expect(labelEntryContext(null)).toBeNull()
  })
})

describe('titleOfGroup', () => {
  it('joins code and name', () => {
    expect(titleOfGroup({ code: '6', name: 'Custos fixos' })).toBe('6 · Custos fixos')
  })
})
