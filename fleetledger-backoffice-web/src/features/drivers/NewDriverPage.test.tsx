import { screen } from '@testing-library/react'
import { DRIVER_ID, driverBody, driverFixture } from '@/test/fakes/drivers'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { renderApp } from '@/test/renderApp'

const CREATE_ROUTE = 'POST /api/v1/drivers'

async function fillForm(user: ReturnType<typeof renderApp>['user']): Promise<void> {
  await user.type(await screen.findByRole('textbox', { name: 'Nome' }), 'João da Silva')
  await user.type(screen.getByRole('textbox', { name: 'CPF' }), '529.982.247-25')
  await user.type(screen.getByRole('textbox', { name: 'Telefone' }), '11987654321')
  await user.clear(screen.getByRole('textbox', { name: 'Comissão padrão (%)' }))
  await user.type(screen.getByRole('textbox', { name: 'Comissão padrão (%)' }), '12,5')
  await user.type(screen.getByRole('textbox', { name: 'Número da CNH' }), '12345678901')
  await user.type(screen.getByLabelText('Validade da CNH'), '2028-05-10')
}

describe('NewDriverPage', () => {
  it('creates the driver and opens its detail', async () => {
    const backend = new FakeHttpBackend()
      .respondWithJson(CREATE_ROUTE, { status: 201, body: driverBody(driverFixture(), 201) })
      .respondWithJson(`GET /api/v1/drivers/${DRIVER_ID}`, { status: 200, body: driverBody(driverFixture()) })
    const { user } = renderApp(backend, '/motoristas/novo')

    await fillForm(user)
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('heading', { level: 1, name: 'João da Silva' })).toBeInTheDocument()
    const created = backend.receivedRequests.find((request) => request.method === 'POST')
    expect(await created?.json()).toMatchObject({
      cpf: '529.982.247-25',
      cnhCategory: 'E',
      cnhExpiresOn: '2028-05-10',
      toxicologyExamExpiresOn: null,
      defaultCommissionPercent: 12.5,
    })
  })

  it('asks for the CNH expiration before calling the API', async () => {
    const backend = new FakeHttpBackend()
    const { user } = renderApp(backend, '/motoristas/novo')

    await user.click(await screen.findByRole('button', { name: 'Salvar' }))

    expect(screen.getByLabelText('Validade da CNH')).toHaveAccessibleDescription('Informe a validade da CNH.')
    expect(backend.receivedRequests.some((request) => request.method === 'POST')).toBe(false)
  })

  it('shows a commission out of range on its field', async () => {
    const backend = new FakeHttpBackend().respondWithJson(CREATE_ROUTE, {
      status: 400,
      body: {
        errors: { DefaultCommissionPercent: ['Comissão deve estar entre 0% e 100% (recebido: 120%)'] },
      },
    })
    const { user } = renderApp(backend, '/motoristas/novo')

    await fillForm(user)
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('textbox', { name: 'Comissão padrão (%)' })).toHaveAccessibleDescription(
      /entre 0% e 100%/,
    )
  })

  it('shows the duplicate CPF refusal', async () => {
    const backend = new FakeHttpBackend().respondWithJson(CREATE_ROUTE, {
      status: 409,
      body: { data: null, code: 409, message: 'Já existe um motorista com o CPF 52998224725.' },
    })
    const { user } = renderApp(backend, '/motoristas/novo')

    await fillForm(user)
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('alert')).toHaveTextContent(
      'Já existe um motorista com o CPF 52998224725.',
    )
  })
})
