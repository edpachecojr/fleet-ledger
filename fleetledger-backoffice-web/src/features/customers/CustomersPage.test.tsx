import { screen, waitFor, within } from '@testing-library/react'
import { customerFixture, customerPageBody } from '@/test/fakes/customers'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { renderApp } from '@/test/renderApp'

const LIST_ROUTE = 'GET /api/v1/customers'

function backendWith(customers = [customerFixture()]): FakeHttpBackend {
  return new FakeHttpBackend().respondWithJson(LIST_ROUTE, { status: 200, body: customerPageBody(customers) })
}

function lastListUrl(backend: FakeHttpBackend): URL {
  const requests = backend.receivedRequests.filter(
    (request) => new URL(request.url).pathname === '/api/v1/customers',
  )
  return new URL(requests.at(-1)?.url ?? '')
}

describe('CustomersPage', () => {
  it('lists customers with masked CNPJ, billing cycle, term and contact', async () => {
    renderApp(
      backendWith([
        customerFixture(),
        customerFixture({
          id: '2',
          corporateName: 'Sem Contato SA',
          phone: null,
          email: null,
          paymentTermDays: null,
          billingCycle: 'Weekly',
        }),
      ]),
      '/clientes',
    )

    const row = await screen.findByRole('row', { name: /Agro Sul Ltda/ })
    expect(within(row).getByText('11.222.333/0001-81')).toBeInTheDocument()
    expect(within(row).getByText('Mensal')).toBeInTheDocument()
    expect(within(row).getByText('30 dias')).toBeInTheDocument()
    expect(within(row).getByText('(11) 98765-4321')).toBeInTheDocument()
    const withoutContact = screen.getByRole('row', { name: /Sem Contato SA/ })
    expect(within(withoutContact).getByText('Semanal')).toBeInTheDocument()
    expect(within(withoutContact).getAllByText('—')).toHaveLength(2)
  })

  it('searches by name or CNPJ and lists only active customers by default', async () => {
    const backend = backendWith()
    const { user } = renderApp(backend, '/clientes')
    await screen.findByRole('row', { name: /Agro Sul Ltda/ })

    await user.type(screen.getByRole('searchbox', { name: 'Buscar cliente' }), '11.222')

    await waitFor(() => {
      expect(lastListUrl(backend).searchParams.get('search')).toBe('11.222')
    })
    expect(lastListUrl(backend).searchParams.get('isActive')).toBe('true')
  })

  it('shows the empty state when nothing matches', async () => {
    renderApp(backendWith([]), '/clientes')

    expect(await screen.findByText('Nenhum cliente encontrado')).toBeInTheDocument()
  })
})
