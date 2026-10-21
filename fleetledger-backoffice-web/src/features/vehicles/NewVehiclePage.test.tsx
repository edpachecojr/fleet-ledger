import { screen } from '@testing-library/react'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { VEHICLE_ID, vehicleBody, vehicleFixture } from '@/test/fakes/vehicles'
import { renderApp } from '@/test/renderApp'

const CREATE_ROUTE = 'POST /api/v1/vehicles'

async function fillForm(user: ReturnType<typeof renderApp>['user']): Promise<void> {
  await user.type(await screen.findByRole('textbox', { name: 'Placa' }), 'abc-1d23')
  await user.type(screen.getByRole('textbox', { name: 'Modelo' }), 'Volvo FH 540')
  await user.type(screen.getByRole('textbox', { name: 'Capacidade (kg)' }), '27.000')
  await user.clear(screen.getByRole('textbox', { name: 'Hodômetro inicial (km)' }))
  await user.type(screen.getByRole('textbox', { name: 'Hodômetro inicial (km)' }), '100000')
  await user.clear(screen.getByRole('textbox', { name: 'Taxa de provisão de manutenção (R$/km)' }))
  await user.type(screen.getByRole('textbox', { name: 'Taxa de provisão de manutenção (R$/km)' }), '0,15')
}

describe('NewVehiclePage', () => {
  it('creates the vehicle and opens its detail', async () => {
    const backend = new FakeHttpBackend()
      .respondWithJson(CREATE_ROUTE, { status: 201, body: vehicleBody(vehicleFixture(), 201) })
      .respondWithJson(`GET /api/v1/vehicles/${VEHICLE_ID}`, {
        status: 200,
        body: vehicleBody(vehicleFixture()),
      })
    const { user } = renderApp(backend, '/veiculos/novo')

    await fillForm(user)
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('heading', { level: 1, name: 'ABC1D23' })).toBeInTheDocument()
    const created = backend.receivedRequests.find((request) => request.method === 'POST')
    expect(await created?.json()).toEqual({
      plate: 'abc-1d23',
      model: 'Volvo FH 540',
      capacityKg: 27000,
      initialOdometerKm: 100000,
      maintenanceProvisionRatePerKm: 0.15,
    })
  })

  it('shows the API validation message on the field', async () => {
    const backend = new FakeHttpBackend().respondWithJson(CREATE_ROUTE, {
      status: 400,
      body: { errors: { Plate: ["Placa 'ABC-1D23' inválida. Esperado: formato antigo ou Mercosul"] } },
    })
    const { user } = renderApp(backend, '/veiculos/novo')

    await fillForm(user)
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('textbox', { name: 'Placa' })).toHaveAccessibleDescription(/inválida/)
    expect(screen.getByRole('alert')).toHaveTextContent('Revise os campos destacados.')
  })

  it('shows the duplicate plate refusal', async () => {
    const backend = new FakeHttpBackend().respondWithJson(CREATE_ROUTE, {
      status: 409,
      body: { data: null, code: 409, message: 'Já existe um veículo com a placa ABC1D23.' },
    })
    const { user } = renderApp(backend, '/veiculos/novo')

    await fillForm(user)
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('alert')).toHaveTextContent('Já existe um veículo com a placa ABC1D23.')
  })

  it('flags an unreadable number without calling the API', async () => {
    const backend = new FakeHttpBackend()
    const { user } = renderApp(backend, '/veiculos/novo')

    await user.type(await screen.findByRole('textbox', { name: 'Capacidade (kg)' }), 'muito')
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(screen.getByRole('textbox', { name: 'Capacidade (kg)' })).toHaveAccessibleDescription(
      'Informe um número.',
    )
    expect(backend.receivedRequests.some((request) => request.method === 'POST')).toBe(false)
  })
})
