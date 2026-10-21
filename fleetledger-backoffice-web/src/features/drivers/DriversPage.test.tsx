import { screen, waitFor, within } from '@testing-library/react'
import { driverFixture, driverPageBody } from '@/test/fakes/drivers'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { renderApp } from '@/test/renderApp'

const LIST_ROUTE = 'GET /api/v1/drivers'

function backendWith(drivers = [driverFixture()]): FakeHttpBackend {
  return new FakeHttpBackend().respondWithJson(LIST_ROUTE, { status: 200, body: driverPageBody(drivers) })
}

function lastListUrl(backend: FakeHttpBackend): URL {
  const requests = backend.receivedRequests.filter(
    (request) => new URL(request.url).pathname === '/api/v1/drivers',
  )
  return new URL(requests.at(-1)?.url ?? '')
}

describe('DriversPage', () => {
  it('shows each driver document status as computed by the API', async () => {
    renderApp(
      backendWith([
        driverFixture(),
        driverFixture({ id: '2', name: 'Maria Souza', cpf: '11144477735', documentStatus: 'Expired' }),
        driverFixture({
          id: '3',
          name: 'Pedro Lima',
          cpf: '39053344705',
          documentStatus: 'Regular',
          toxicologyExamExpiresOn: null,
        }),
      ]),
      '/motoristas',
    )

    const joao = await screen.findByRole('row', { name: /João da Silva/ })
    expect(within(joao).getByText('Vencendo')).toBeInTheDocument()
    expect(within(joao).getByText('529.982.247-25')).toBeInTheDocument()
    expect(within(joao).getByText('12,50%')).toBeInTheDocument()
    expect(within(joao).getByText('Vence 10/05/2028')).toBeInTheDocument()
    expect(within(screen.getByRole('row', { name: /Maria Souza/ })).getByText('Vencido')).toBeInTheDocument()
    expect(within(screen.getByRole('row', { name: /Pedro Lima/ })).getByText('Regular')).toBeInTheDocument()
  })

  it('filters drivers with expired documents', async () => {
    const backend = backendWith()
    const { user } = renderApp(backend, '/motoristas')
    await screen.findByRole('row', { name: /João da Silva/ })

    await user.selectOptions(screen.getByRole('combobox', { name: 'Documentos' }), 'Vencido')

    await waitFor(() => {
      expect(lastListUrl(backend).searchParams.get('documentStatus')).toBe('Expired')
    })
  })

  it('filters drivers with documents expiring soon', async () => {
    const backend = backendWith()
    const { user } = renderApp(backend, '/motoristas')
    await screen.findByRole('row', { name: /João da Silva/ })

    await user.selectOptions(screen.getByRole('combobox', { name: 'Documentos' }), 'Vencendo')

    await waitFor(() => {
      expect(lastListUrl(backend).searchParams.get('documentStatus')).toBe('ExpiringSoon')
    })
  })

  it('searches by name or CPF among active drivers', async () => {
    const backend = backendWith()
    const { user } = renderApp(backend, '/motoristas')
    await screen.findByRole('row', { name: /João da Silva/ })

    await user.type(screen.getByRole('searchbox', { name: 'Buscar motorista' }), '529.982')

    await waitFor(() => {
      expect(lastListUrl(backend).searchParams.get('search')).toBe('529.982')
    })
    expect(lastListUrl(backend).searchParams.get('isActive')).toBe('true')
    expect(lastListUrl(backend).searchParams.has('documentStatus')).toBe(false)
  })

  it('shows the empty state when nothing matches', async () => {
    renderApp(backendWith([]), '/motoristas')

    expect(await screen.findByText('Nenhum motorista encontrado')).toBeInTheDocument()
  })
})
