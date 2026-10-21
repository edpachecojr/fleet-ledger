import { act, screen } from '@testing-library/react'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { systemHealthReport } from '@/test/fakes/systemHealthReports'
import { renderApp } from '@/test/renderApp'
import { EIGHT_HOURS_MS, signedInSession, signedOutSession, TEST_ACCESS_TOKEN } from '@/test/testSessions'

const LOGIN_ROUTE = 'POST /api/v1/auth/login'

const healthyBackend = (): FakeHttpBackend =>
  new FakeHttpBackend().respondWithJson('GET /api/health', {
    status: 200,
    body: systemHealthReport('Healthy'),
  })

async function fillAndSubmit(user: ReturnType<typeof renderApp>['user'], password: string): Promise<void> {
  await user.type(await screen.findByLabelText('E-mail'), 'admin@fleetledger.local')
  await user.type(screen.getByLabelText('Senha'), password)
  await user.click(screen.getByRole('button', { name: 'Entrar' }))
}

describe('LoginPage', () => {
  it('redirects to login when opening a screen without a session', async () => {
    renderApp(healthyBackend(), '/', signedOutSession().session)

    expect(await screen.findByRole('heading', { name: 'Entrar' })).toBeInTheDocument()
  })

  it('opens the backoffice after a valid login', async () => {
    const backend = healthyBackend().respondWithJson(LOGIN_ROUTE, {
      status: 200,
      body: { data: TEST_ACCESS_TOKEN, message: null, code: 200 },
    })
    const { user } = renderApp(backend, '/', signedOutSession().session)

    await fillAndSubmit(user, 'Senha-Forte-123')

    expect(await screen.findByRole('heading', { level: 1, name: 'Visão geral' })).toBeInTheDocument()
  })

  it('sends the access token on API calls after login', async () => {
    const { session } = signedOutSession()
    const backend = healthyBackend().respondWithJson(LOGIN_ROUTE, {
      status: 200,
      body: { data: TEST_ACCESS_TOKEN, message: null, code: 200 },
    })
    const { user } = renderApp(backend, '/', session)

    await fillAndSubmit(user, 'Senha-Forte-123')
    await screen.findByText('Banco de dados')

    const healthRequest = backend.receivedRequests.find((request) => request.url.endsWith('/health'))
    expect(healthRequest?.headers.get('Authorization')).toBe(`Bearer ${TEST_ACCESS_TOKEN.accessToken}`)
  })

  it('shows the generic message and stays on login when credentials are refused', async () => {
    const backend = healthyBackend().respondWithJson(LOGIN_ROUTE, {
      status: 401,
      body: { data: null, message: 'E-mail ou senha inválidos.', code: 401 },
    })
    const { user } = renderApp(backend, '/', signedOutSession().session)

    await fillAndSubmit(user, 'senha-errada')

    expect(await screen.findByRole('alert')).toHaveTextContent('E-mail ou senha inválidos.')
    expect(screen.getByRole('heading', { name: 'Entrar' })).toBeInTheDocument()
  })

  it('tells an unavailable API apart from wrong credentials', async () => {
    const backend = healthyBackend().respondWithText(LOGIN_ROUTE, { status: 502, body: 'Bad Gateway' })
    const { user } = renderApp(backend, '/', signedOutSession().session)

    await fillAndSubmit(user, 'Senha-Forte-123')

    expect(await screen.findByRole('alert')).toHaveTextContent('Não foi possível entrar agora')
  })

  it('skips the login page when the session is already active', async () => {
    renderApp(healthyBackend(), '/login')

    expect(await screen.findByRole('heading', { level: 1, name: 'Visão geral' })).toBeInTheDocument()
  })
})

describe('session end', () => {
  afterEach(() => {
    vi.useRealTimers()
  })

  it('returns to login after logout', async () => {
    const { user } = renderApp(healthyBackend())

    await user.click(await screen.findByRole('button', { name: 'Sair' }))

    expect(await screen.findByRole('heading', { name: 'Entrar' })).toBeInTheDocument()
  })

  it('returns to login when the API rejects the token', async () => {
    renderApp(new FakeHttpBackend().respondWithJson('GET /api/health', { status: 401, body: {} }))

    expect(await screen.findByRole('heading', { name: 'Entrar' })).toBeInTheDocument()
  })

  it('returns to login when the token expires', async () => {
    vi.useFakeTimers({ shouldAdvanceTime: true })
    const { session, clock } = signedInSession()
    renderApp(healthyBackend(), '/', session)
    await screen.findByRole('heading', { level: 1, name: 'Visão geral' })

    act(() => {
      clock.advance(EIGHT_HOURS_MS)
      vi.advanceTimersByTime(EIGHT_HOURS_MS)
    })

    expect(await screen.findByRole('heading', { name: 'Entrar' })).toBeInTheDocument()
  })
})
