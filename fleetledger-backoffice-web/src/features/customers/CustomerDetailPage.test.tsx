import { screen, waitFor, within } from '@testing-library/react'
import { CUSTOMER_ID, customerBody, customerFixture } from '@/test/fakes/customers'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { renderApp } from '@/test/renderApp'
import type { Customer } from './customer'

const DETAIL_ROUTE = `GET /api/v1/customers/${CUSTOMER_ID}`
const DETAIL_PAGE = `/clientes/${CUSTOMER_ID}`

function backendWith(customer: Customer = customerFixture()): FakeHttpBackend {
  return new FakeHttpBackend().respondWithJson(DETAIL_ROUTE, { status: 200, body: customerBody(customer) })
}

describe('CustomerDetailPage', () => {
  it('shows the billing cycle and term, with the CNPJ locked', async () => {
    renderApp(backendWith(), DETAIL_PAGE)

    const summary = (await screen.findByRole('heading', { name: 'Faturamento' })).closest(
      'section',
    ) as HTMLElement
    expect(within(summary).getByText('Mensal')).toBeInTheDocument()
    expect(within(summary).getByText('30 dias')).toBeInTheDocument()
    expect(screen.getByRole('textbox', { name: 'CNPJ' })).toBeDisabled()
    expect(screen.getByRole('textbox', { name: 'CNPJ' })).toHaveValue('11.222.333/0001-81')
  })

  it('saves the profile without the CNPJ', async () => {
    const backend = backendWith().respondWithJson(`PUT /api/v1/customers/${CUSTOMER_ID}`, {
      status: 200,
      body: customerBody(customerFixture({ billingCycle: 'Weekly' })),
    })
    const { user } = renderApp(backend, DETAIL_PAGE)

    await user.selectOptions(await screen.findByRole('combobox', { name: 'Ciclo de faturamento' }), 'Semanal')
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('status')).toHaveTextContent('Cliente atualizado.')
    const update = backend.receivedRequests.find((request) => request.method === 'PUT')
    const body: unknown = await update?.json()
    expect(body).toMatchObject({ billingCycle: 'Weekly', paymentTermDays: 30 })
    expect(body).not.toHaveProperty('cnpj')
  })

  it('deactivates after confirmation', async () => {
    const backend = backendWith().respondWithJson(`POST /api/v1/customers/${CUSTOMER_ID}/deactivate`, {
      status: 200,
      body: customerBody(customerFixture({ isActive: false })),
    })
    const { user } = renderApp(backend, DETAIL_PAGE)

    await user.click(await screen.findByRole('button', { name: 'Inativar' }))
    await user.click(
      within(screen.getByRole('alertdialog', { name: 'Inativar cliente?' })).getByRole('button', {
        name: 'Inativar',
      }),
    )

    await waitFor(() => {
      expect(backend.receivedRequests.some((request) => request.url.endsWith('/deactivate'))).toBe(true)
    })
  })

  it('reactivates an inactive customer', async () => {
    const backend = backendWith(customerFixture({ isActive: false })).respondWithJson(
      `POST /api/v1/customers/${CUSTOMER_ID}/reactivate`,
      { status: 200, body: customerBody(customerFixture()) },
    )
    const { user } = renderApp(backend, DETAIL_PAGE)

    await user.click(await screen.findByRole('button', { name: 'Reativar' }))

    await waitFor(() => {
      expect(backend.receivedRequests.some((request) => request.url.endsWith('/reactivate'))).toBe(true)
    })
  })

  it('tells when the customer does not exist', async () => {
    renderApp(
      new FakeHttpBackend().respondWithJson(DETAIL_ROUTE, {
        status: 404,
        body: { data: null, code: 404, message: 'x' },
      }),
      DETAIL_PAGE,
    )

    expect(await screen.findByText('Cliente não encontrado')).toBeInTheDocument()
  })
})
