import { EMPTY_DRIVER_FORM, formValuesOf, parseCreateDriverForm, parseUpdateDriverForm } from './driverForm'

const FILLED = {
  ...EMPTY_DRIVER_FORM,
  name: 'João da Silva',
  cpf: '529.982.247-25',
  phone: '(11) 98765-4321',
  cnhNumber: '12345678901',
  cnhExpiresOn: '2028-05-10',
  defaultCommissionPercent: '12,5',
}

describe('parseCreateDriverForm', () => {
  it('sends the commission as a number and a blank toxicology date as null', () => {
    const parsed = parseCreateDriverForm(FILLED)

    expect(parsed).toEqual({
      kind: 'parsed',
      request: {
        name: 'João da Silva',
        cpf: '529.982.247-25',
        phone: '(11) 98765-4321',
        cnhNumber: '12345678901',
        cnhCategory: 'E',
        cnhExpiresOn: '2028-05-10',
        toxicologyExamExpiresOn: null,
        defaultCommissionPercent: 12.5,
        bankDetails: '',
      },
    })
  })

  it('requires the CNH expiration and a readable commission', () => {
    expect(parseCreateDriverForm({ ...FILLED, cnhExpiresOn: '', defaultCommissionPercent: 'dez' })).toEqual({
      kind: 'invalid',
      fieldErrors: {
        cnhExpiresOn: 'Informe a validade da CNH.',
        defaultCommissionPercent: 'Informe um número.',
      },
    })
  })
})

describe('parseUpdateDriverForm', () => {
  it('never sends the CPF', () => {
    const parsed = parseUpdateDriverForm(FILLED)

    expect(parsed.kind === 'parsed' && 'cpf' in parsed.request).toBe(false)
  })
})

describe('formValuesOf', () => {
  it('masks CPF and phone and formats the commission', () => {
    expect(
      formValuesOf({
        cpf: '52998224725',
        phone: '11987654321',
        defaultCommissionPercent: 12.5,
        cnhExpiresOn: '2028-05-10',
      }),
    ).toMatchObject({
      cpf: '529.982.247-25',
      phone: '(11) 98765-4321',
      defaultCommissionPercent: '12,50',
      toxicologyExamExpiresOn: '',
    })
  })
})
