import { screen, waitFor, within } from '@testing-library/react'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { VEHICLE_ID, vehicleBody, vehicleFixture } from '@/test/fakes/vehicles'
import { renderApp } from '@/test/renderApp'
import type { Vehicle } from './vehicle'

const DETAIL_ROUTE = `GET /api/v1/vehicles/${VEHICLE_ID}`
const DETAIL_PAGE = `/veiculos/${VEHICLE_ID}`

function backendWith(vehicle: Vehicle = vehicleFixture()): FakeHttpBackend {
  return new FakeHttpBackend().respondWithJson(DETAIL_ROUTE, { status: 200, body: vehicleBody(vehicle) })
}

describe('VehicleDetailPage', () => {
  it('highlights status and odometer, with plate locked and no odometer field', async () => {
    renderApp(backendWith(), DETAIL_PAGE)

    const summary = (await screen.findByRole('heading', { name: 'Situação' })).closest(
      'section',
    ) as HTMLElement
    expect(within(summary).getByText('Disponível')).toBeInTheDocument()
    expect(within(summary).getByText('125.000 km')).toBeInTheDocument()
    expect(screen.getByRole('textbox', { name: 'Placa' })).toBeDisabled()
    expect(screen.queryByRole('textbox', { name: /Hodômetro/ })).not.toBeInTheDocument()
  })

  it('saves only the editable fields', async () => {
    const backend = backendWith().respondWithJson(`PUT /api/v1/vehicles/${VEHICLE_ID}`, {
      status: 200,
      body: vehicleBody(vehicleFixture({ model: 'Scania R 450' })),
    })
    const { user } = renderApp(backend, DETAIL_PAGE)

    const model = await screen.findByRole('textbox', { name: 'Modelo' })
    await user.clear(model)
    await user.type(model, 'Scania R 450')
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('status')).toHaveTextContent('Veículo atualizado.')
    const update = backend.receivedRequests.find((request) => request.method === 'PUT')
    expect(await update?.json()).toEqual({
      model: 'Scania R 450',
      capacityKg: 27000,
      maintenanceProvisionRatePerKm: 0.15,
    })
  })

  it('asks for confirmation before deactivating', async () => {
    const backend = backendWith().respondWithJson(`POST /api/v1/vehicles/${VEHICLE_ID}/deactivate`, {
      status: 200,
      body: vehicleBody(vehicleFixture({ isActive: false })),
    })
    const { user } = renderApp(backend, DETAIL_PAGE)

    await user.click(await screen.findByRole('button', { name: 'Inativar' }))
    const dialog = screen.getByRole('alertdialog', { name: 'Inativar veículo?' })
    await user.click(within(dialog).getByRole('button', { name: 'Inativar' }))

    await waitFor(() => {
      expect(backend.receivedRequests.some((request) => request.url.endsWith('/deactivate'))).toBe(true)
    })
    expect(screen.queryByRole('alertdialog')).not.toBeInTheDocument()
  })

  it('reactivates an inactive vehicle', async () => {
    const backend = backendWith(vehicleFixture({ isActive: false })).respondWithJson(
      `POST /api/v1/vehicles/${VEHICLE_ID}/reactivate`,
      { status: 200, body: vehicleBody(vehicleFixture()) },
    )
    const { user } = renderApp(backend, DETAIL_PAGE)

    await user.click(await screen.findByRole('button', { name: 'Reativar' }))

    await waitFor(() => {
      expect(backend.receivedRequests.some((request) => request.url.endsWith('/reactivate'))).toBe(true)
    })
  })

  it('puts an available vehicle in maintenance', async () => {
    const backend = backendWith().respondWithJson(`PUT /api/v1/vehicles/${VEHICLE_ID}/status`, {
      status: 200,
      body: vehicleBody(vehicleFixture({ status: 'InMaintenance' })),
    })
    const { user } = renderApp(backend, DETAIL_PAGE)

    await user.click(await screen.findByRole('button', { name: 'Colocar em manutenção' }))

    await waitFor(() => {
      expect(backend.receivedRequests.some((request) => request.url.endsWith('/status'))).toBe(true)
    })
    const change = backend.receivedRequests.find((request) => request.url.endsWith('/status'))
    expect(await change?.json()).toEqual({ status: 'InMaintenance' })
  })

  it('releases a vehicle in maintenance', async () => {
    renderApp(backendWith(vehicleFixture({ status: 'InMaintenance' })), DETAIL_PAGE)

    expect(await screen.findByRole('button', { name: 'Liberar para uso' })).toBeInTheDocument()
  })

  it('offers no manual status change while the vehicle is on trip', async () => {
    renderApp(backendWith(vehicleFixture({ status: 'OnTrip' })), DETAIL_PAGE)

    await screen.findByRole('heading', { level: 1, name: 'ABC1D23' })

    expect(screen.queryByRole('button', { name: /manutenção|Liberar/ })).not.toBeInTheDocument()
    expect(screen.getAllByText('Em viagem').length).toBeGreaterThan(0)
  })

  it('shows the API refusal of a status change', async () => {
    const backend = backendWith().respondWithJson(`PUT /api/v1/vehicles/${VEHICLE_ID}/status`, {
      status: 409,
      body: { data: null, code: 409, message: 'O veículo ABC1D23 está em viagem.' },
    })
    const { user } = renderApp(backend, DETAIL_PAGE)

    await user.click(await screen.findByRole('button', { name: 'Colocar em manutenção' }))

    expect(await screen.findByRole('alert')).toHaveTextContent('O veículo ABC1D23 está em viagem.')
  })

  it('tells when the vehicle does not exist', async () => {
    renderApp(
      new FakeHttpBackend().respondWithJson(DETAIL_ROUTE, {
        status: 404,
        body: { data: null, code: 404, message: 'Veículo não encontrado.' },
      }),
      DETAIL_PAGE,
    )

    expect(await screen.findByText('Veículo não encontrado')).toBeInTheDocument()
  })
})
