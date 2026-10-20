import { screen, within } from '@testing-library/react'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { systemHealthReport } from '@/test/fakes/systemHealthReports'
import { renderApp } from '@/test/renderApp'

const healthyBackend = (): FakeHttpBackend =>
  new FakeHttpBackend().respondWithJson('GET /api/health', {
    status: 200,
    body: systemHealthReport('Healthy'),
  })

describe('AppShell', () => {
  it('renders the brand, the menu and the overview page title', async () => {
    renderApp(healthyBackend())

    expect(await screen.findByRole('heading', { level: 1, name: 'Visão geral' })).toBeInTheDocument()
    const menu = screen.getAllByRole('navigation', { name: 'Menu principal' })[0] as HTMLElement
    expect(within(menu).getByRole('link', { name: 'Visão geral' })).toHaveAttribute('aria-current', 'page')
  })

  it('opens the navigation drawer from the header menu button', async () => {
    const { user } = renderApp(healthyBackend())

    await user.click(screen.getByRole('button', { name: 'Abrir menu' }))

    expect(screen.getByRole('dialog', { name: 'Menu' })).toBeInTheDocument()
  })

  it('shows the not-found page for unknown routes', async () => {
    renderApp(healthyBackend(), '/rota-inexistente')

    expect(await screen.findByText('Página não encontrada')).toBeInTheDocument()
  })
})
