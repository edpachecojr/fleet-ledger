import { screen } from '@testing-library/react'
import { CUSTOMER_ID, customerBody, customerFixture } from '@/test/fakes/customers'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { renderApp } from '@/test/renderApp'

const CREATE_ROUTE = 'POST /api/v1/customers'

async function fillRequired(user: ReturnType<typeof renderApp>['user']): Promise<void> {
  await user.type(await screen.findByRole('textbox', { name: 'Razão social' }), 'Agro Sul Ltda')
  await user.type(screen.getByRole('textbox', { name: 'CNPJ' }), '11.222.333/0001-81')
  await user.selectOptions(screen.getByRole('combobox', { name: 'Ciclo de faturamento' }), 'Quinzenal')
}

describe('NewCustomerPage', () => {
  it('creates the customer with only the required fields and opens its detail', async () => {
    const backend = new FakeHttpBackend()
      .respondWithJson(CREATE_ROUTE, { status: 201, body: customerBody(customerFixture(), 201) })
      .respondWithJson(`GET /api/v1/customers/${CUSTOMER_ID}`, {
        status: 200,
        body: customerBody(customerFixture()),
      })
    const { user } = renderApp(backend, '/clientes/novo')

    await fillRequired(user)
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('heading', { level: 1, name: 'Agro Sul Ltda' })).toBeInTheDocument()
    const created = backend.receivedRequests.find((request) => request.method === 'POST')
    expect(await created?.json()).toMatchObject({
      corporateName: 'Agro Sul Ltda',
      cnpj: '11.222.333/0001-81',
      billingCycle: 'Biweekly',
      paymentTermDays: null,
    })
  })

  it('shows an invalid CNPJ on its field', async () => {
    const backend = new FakeHttpBackend().respondWithJson(CREATE_ROUTE, {
      status: 400,
      body: { errors: { Cnpj: ["CNPJ '11.222.333/0001-82' inválido."] } },
    })
    const { user } = renderApp(backend, '/clientes/novo')

    await fillRequired(user)
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('textbox', { name: 'CNPJ' })).toHaveAccessibleDescription(/inválido/)
  })

  it('shows the duplicate CNPJ refusal', async () => {
    const backend = new FakeHttpBackend().respondWithJson(CREATE_ROUTE, {
      status: 409,
      body: { data: null, code: 409, message: 'Já existe um cliente com o CNPJ 11222333000181.' },
    })
    const { user } = renderApp(backend, '/clientes/novo')

    await fillRequired(user)
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('alert')).toHaveTextContent(
      'Já existe um cliente com o CNPJ 11222333000181.',
    )
  })
})
