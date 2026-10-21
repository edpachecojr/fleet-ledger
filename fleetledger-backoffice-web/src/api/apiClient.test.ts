import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { systemHealthReport } from '@/test/fakes/systemHealthReports'
import { TEST_API_BASE_URL } from '@/test/renderApp'
import { anonymousAccessTokenSource } from './accessTokenSource'
import { createApiClient } from './apiClient'

describe('createApiClient', () => {
  const backend = (): FakeHttpBackend =>
    new FakeHttpBackend().respondWithJson('GET /api/health', {
      status: 200,
      body: systemHealthReport('Healthy'),
    })

  it('sends the access token as a bearer header when there is one', async () => {
    const http = backend()
    const client = createApiClient({
      baseUrl: TEST_API_BASE_URL,
      accessTokenSource: { getAccessToken: () => 'jwt-token', discardAccessToken: () => undefined },
      fetch: http.fetch,
    })

    await client.GET('/health')

    expect(http.receivedRequests[0]?.headers.get('Authorization')).toBe('Bearer jwt-token')
  })

  it('sends no authorization header when anonymous', async () => {
    const http = backend()
    const client = createApiClient({
      baseUrl: TEST_API_BASE_URL,
      accessTokenSource: anonymousAccessTokenSource,
      fetch: http.fetch,
    })

    await client.GET('/health')

    expect(http.receivedRequests[0]?.headers.has('Authorization')).toBe(false)
  })

  it('discards the token when the API rejects it with 401', async () => {
    const http = new FakeHttpBackend().respondWithJson('GET /api/health', { status: 401, body: {} })
    const discardAccessToken = vi.fn()
    const client = createApiClient({
      baseUrl: TEST_API_BASE_URL,
      accessTokenSource: { getAccessToken: () => 'expired-token', discardAccessToken },
      fetch: http.fetch,
    })

    await client.GET('/health')

    expect(discardAccessToken).toHaveBeenCalledOnce()
  })

  it('keeps the session when a request without token gets 401 (e.g. wrong password)', async () => {
    const http = new FakeHttpBackend().respondWithJson('GET /api/health', { status: 401, body: {} })
    const discardAccessToken = vi.fn()
    const client = createApiClient({
      baseUrl: TEST_API_BASE_URL,
      accessTokenSource: { getAccessToken: () => null, discardAccessToken },
      fetch: http.fetch,
    })

    await client.GET('/health')

    expect(discardAccessToken).not.toHaveBeenCalled()
  })
})
