import { screen, within } from '@testing-library/react'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { systemHealthReport } from '@/test/fakes/systemHealthReports'
import { renderApp } from '@/test/renderApp'

async function healthCard(): Promise<HTMLElement> {
  const title = await screen.findByRole('heading', { name: 'Status do sistema' })
  return title.closest('section') as HTMLElement
}

describe('SystemHealthCard', () => {
  it('shows API and database as operational when healthy', async () => {
    renderApp(
      new FakeHttpBackend().respondWithJson('GET /api/health', {
        status: 200,
        body: systemHealthReport('Healthy'),
      }),
    )

    const card = await healthCard()

    expect(await within(card).findAllByText('Operacional')).toHaveLength(2)
    expect(within(card).getByText('Banco de dados')).toBeInTheDocument()
  })

  it('shows the database as unavailable when the API answers 503', async () => {
    renderApp(
      new FakeHttpBackend().respondWithJson('GET /api/health', {
        status: 503,
        body: systemHealthReport('Unhealthy'),
      }),
    )

    const card = await healthCard()

    expect(await within(card).findAllByText('Indisponível')).toHaveLength(2)
  })

  it('shows the API as unreachable when there is no health report', async () => {
    renderApp(new FakeHttpBackend().respondWithText('GET /api/health', { status: 502, body: 'Bad Gateway' }))

    const card = await healthCard()

    expect(await within(card).findByText('Inacessível')).toBeInTheDocument()
  })
})
