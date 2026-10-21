import {
  EMPTY_CUSTOMER_FORM,
  formValuesOf,
  parseCreateCustomerForm,
  parseUpdateCustomerForm,
} from './customerForm'

describe('parseCreateCustomerForm', () => {
  it('sends the typed fields and the payment term as a number', () => {
    const parsed = parseCreateCustomerForm({
      ...EMPTY_CUSTOMER_FORM,
      corporateName: 'Agro Sul Ltda',
      cnpj: '11.222.333/0001-81',
      paymentTermDays: '30',
    })

    expect(parsed).toEqual({
      kind: 'parsed',
      request: {
        corporateName: 'Agro Sul Ltda',
        tradeName: '',
        cnpj: '11.222.333/0001-81',
        phone: '',
        email: '',
        billingCycle: 'Monthly',
        paymentTermDays: 30,
      },
    })
  })

  it('treats a blank payment term as not informed', () => {
    const parsed = parseCreateCustomerForm(EMPTY_CUSTOMER_FORM)

    expect(parsed.kind === 'parsed' && parsed.request.paymentTermDays).toBeNull()
  })

  it.each(['trinta', '7,5'])('flags "%s" as an invalid payment term', (term) => {
    expect(parseCreateCustomerForm({ ...EMPTY_CUSTOMER_FORM, paymentTermDays: term })).toEqual({
      kind: 'invalid',
      fieldErrors: { paymentTermDays: 'Informe um número inteiro de dias.' },
    })
  })
})

describe('parseUpdateCustomerForm', () => {
  it('never sends the CNPJ', () => {
    const parsed = parseUpdateCustomerForm({ ...EMPTY_CUSTOMER_FORM, cnpj: '11222333000181' })

    expect(parsed.kind === 'parsed' && 'cnpj' in parsed.request).toBe(false)
  })
})

describe('formValuesOf', () => {
  it('masks CNPJ and phone for display', () => {
    expect(
      formValuesOf({
        corporateName: 'Agro Sul',
        cnpj: '11222333000181',
        phone: '11987654321',
        billingCycle: 'Weekly',
      }),
    ).toMatchObject({
      cnpj: '11.222.333/0001-81',
      phone: '(11) 98765-4321',
      billingCycle: 'Weekly',
      paymentTermDays: '',
    })
  })
})
