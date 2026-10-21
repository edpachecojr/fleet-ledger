import { screen, waitFor, within } from '@testing-library/react'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { vehicleFixture, vehiclePageBody } from '@/test/fakes/vehicles'
import { renderApp } from '@/test/renderApp'

const LIST_ROUTE = 'GET /api/v1/vehicles'

function backendWith(vehicles = [vehicleFixture()]): FakeHttpBackend {
  return new FakeHttpBackend().respondWithJson(LIST_ROUTE, { status: 200, body: vehiclePageBody(vehicles) })
}

function lastListUrl(backend: FakeHttpBackend): URL {
  const requests = backend.receivedRequests.filter(
    (request) => new URL(request.url).pathname === '/api/v1/vehicles',
  )
  return new URL(requests.at(-1)?.url ?? '')
}

describe('VehiclesPage', () => {
  it('lists the fleet with status, activity and formatted numbers', async () => {
    renderApp(
      backendWith([vehicleFixture(), vehicleFixture({ id: '2', plate: 'XYZ9876', status: 'InMaintenance' })]),
      '/veiculos',
    )

    const row = await screen.findByRole('row', { name: /ABC1D23/ })
    expect(within(row).getByText('27.000 kg')).toBeInTheDocument()
    expect(within(row).getByText('125.000 km')).toBeInTheDocument()
    expect(within(row).getByText('Disponível')).toBeInTheDocument()
    expect(within(row).getByText('Ativo')).toBeInTheDocument()
    expect(
      within(screen.getByRole('row', { name: /XYZ9876/ })).getByText('Em manutenção'),
    ).toBeInTheDocument()
  })

  it('lists only active vehicles by default', async () => {
    const backend = backendWith()
    renderApp(backend, '/veiculos')

    await screen.findByRole('row', { name: /ABC1D23/ })

    expect(lastListUrl(backend).searchParams.get('isActive')).toBe('true')
  })

  it('searches by plate or model and filters by activity', async () => {
    const backend = backendWith()
    const { user } = renderApp(backend, '/veiculos')
    await screen.findByRole('row', { name: /ABC1D23/ })

    await user.type(screen.getByRole('searchbox', { name: 'Buscar veículo' }), 'volvo')
    await user.selectOptions(screen.getByRole('combobox', { name: 'Situação' }), 'Inativos')

    await waitFor(() => {
      expect(lastListUrl(backend).searchParams.get('search')).toBe('volvo')
      expect(lastListUrl(backend).searchParams.get('isActive')).toBe('false')
    })
  })

  it('filters by operational status', async () => {
    const backend = backendWith()
    const { user } = renderApp(backend, '/veiculos')
    await screen.findByRole('row', { name: /ABC1D23/ })

    await user.selectOptions(screen.getByRole('combobox', { name: 'Status' }), 'Em manutenção')

    await waitFor(() => {
      expect(lastListUrl(backend).searchParams.get('status')).toBe('InMaintenance')
    })
  })

  it('sends no status filter while showing every status', async () => {
    const backend = backendWith()
    renderApp(backend, '/veiculos')

    await screen.findByRole('row', { name: /ABC1D23/ })

    expect(lastListUrl(backend).searchParams.has('status')).toBe(false)
  })

  it('shows the empty state with the create action when nothing matches', async () => {
    renderApp(backendWith([]), '/veiculos')

    expect(await screen.findByText('Nenhum veículo encontrado')).toBeInTheDocument()
    expect(screen.getAllByRole('link', { name: 'Novo veículo' }).length).toBeGreaterThan(0)
  })

  it('keeps the menu item active on vehicle sub-pages', async () => {
    renderApp(backendWith(), '/veiculos/novo')

    const menu = (await screen.findAllByRole('navigation', { name: 'Menu principal' }))[0] as HTMLElement
    expect(within(menu).getByRole('link', { name: 'Veículos' })).toHaveAttribute('aria-current', 'page')
  })
})
