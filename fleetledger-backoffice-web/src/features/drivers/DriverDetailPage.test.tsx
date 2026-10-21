import { screen, waitFor, within } from '@testing-library/react'
import { DRIVER_ID, driverBody, driverFixture } from '@/test/fakes/drivers'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { renderApp } from '@/test/renderApp'
import type { Driver } from './driver'

const DETAIL_ROUTE = `GET /api/v1/drivers/${DRIVER_ID}`
const DETAIL_PAGE = `/motoristas/${DRIVER_ID}`

function backendWith(driver: Driver = driverFixture()): FakeHttpBackend {
  return new FakeHttpBackend().respondWithJson(DETAIL_ROUTE, { status: 200, body: driverBody(driver) })
}

describe('DriverDetailPage', () => {
  it('highlights the document status with both expirations, CPF locked', async () => {
    renderApp(backendWith(), DETAIL_PAGE)

    const summary = (await screen.findByRole('heading', { name: 'Documentos' })).closest(
      'section',
    ) as HTMLElement
    expect(within(summary).getByText('Vencendo')).toBeInTheDocument()
    expect(within(summary).getByText('10/05/2028')).toBeInTheDocument()
    expect(within(summary).getByText('10/11/2026')).toBeInTheDocument()
    expect(screen.getByRole('textbox', { name: 'CPF' })).toBeDisabled()
  })

  it('saves a new default commission without the CPF', async () => {
    const backend = backendWith().respondWithJson(`PUT /api/v1/drivers/${DRIVER_ID}`, {
      status: 200,
      body: driverBody(driverFixture({ defaultCommissionPercent: 15 })),
    })
    const { user } = renderApp(backend, DETAIL_PAGE)

    const commission = await screen.findByRole('textbox', { name: 'Comissão padrão (%)' })
    await user.clear(commission)
    await user.type(commission, '15')
    await user.click(screen.getByRole('button', { name: 'Salvar' }))

    expect(await screen.findByRole('status')).toHaveTextContent('Motorista atualizado.')
    const update = backend.receivedRequests.find((request) => request.method === 'PUT')
    const body: unknown = await update?.json()
    expect(body).toMatchObject({ defaultCommissionPercent: 15, toxicologyExamExpiresOn: '2026-11-10' })
    expect(body).not.toHaveProperty('cpf')
  })

  it('deactivates after confirmation', async () => {
    const backend = backendWith().respondWithJson(`POST /api/v1/drivers/${DRIVER_ID}/deactivate`, {
      status: 200,
      body: driverBody(driverFixture({ isActive: false })),
    })
    const { user } = renderApp(backend, DETAIL_PAGE)

    await user.click(await screen.findByRole('button', { name: 'Inativar' }))
    await user.click(
      within(screen.getByRole('alertdialog', { name: 'Inativar motorista?' })).getByRole('button', {
        name: 'Inativar',
      }),
    )

    await waitFor(() => {
      expect(backend.receivedRequests.some((request) => request.url.endsWith('/deactivate'))).toBe(true)
    })
  })

  it('tells when the driver does not exist', async () => {
    renderApp(
      new FakeHttpBackend().respondWithJson(DETAIL_ROUTE, {
        status: 404,
        body: { data: null, code: 404, message: 'x' },
      }),
      DETAIL_PAGE,
    )

    expect(await screen.findByText('Motorista não encontrado')).toBeInTheDocument()
  })
})
