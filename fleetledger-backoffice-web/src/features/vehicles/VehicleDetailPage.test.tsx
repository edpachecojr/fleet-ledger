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
