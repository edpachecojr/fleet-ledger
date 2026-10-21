import { formatCnpj, formatCpf, formatPhone } from './documentFormat'

describe('formatCnpj', () => {
  it('masks 14 digits and keeps anything else as is', () => {
    expect(formatCnpj('11222333000181')).toBe('11.222.333/0001-81')
    expect(formatCnpj('123')).toBe('123')
    expect(formatCnpj(null)).toBe('')
  })
})

describe('formatCpf', () => {
  it('masks 11 digits', () => {
    expect(formatCpf('52998224725')).toBe('529.982.247-25')
  })
})

describe('formatPhone', () => {
  it('masks mobile and landline numbers with area code', () => {
    expect(formatPhone('11987654321')).toBe('(11) 98765-4321')
    expect(formatPhone('1133334444')).toBe('(11) 3333-4444')
    expect(formatPhone(undefined)).toBe('')
  })
})
