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
      accessTokenSource: { getAccessToken: () => 'jwt-token' },
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
})
