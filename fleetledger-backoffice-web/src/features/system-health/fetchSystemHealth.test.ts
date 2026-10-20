import { ApiRequestError } from '@/api/ApiRequestError'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { systemHealthReport } from '@/test/fakes/systemHealthReports'
import { createTestApiClient } from '@/test/renderApp'
import { fetchSystemHealth } from './fetchSystemHealth'

describe('fetchSystemHealth', () => {
  it('returns the report when the API is healthy', async () => {
    const backend = new FakeHttpBackend().respondWithJson('GET /api/health', {
      status: 200,
      body: systemHealthReport('Healthy'),
    })

    const health = await fetchSystemHealth(createTestApiClient(backend))

    expect(health.status).toBe('Healthy')
  })

  it('returns the report on 503 so the caller sees which dependency failed', async () => {
    const backend = new FakeHttpBackend().respondWithJson('GET /api/health', {
      status: 503,
      body: systemHealthReport('Unhealthy'),
    })

    const health = await fetchSystemHealth(createTestApiClient(backend))

    expect(health.checks[0]?.status).toBe('Unhealthy')
  })

  it('throws with context when the response is not a health report', async () => {
    const backend = new FakeHttpBackend().respondWithText('GET /api/health', {
      status: 502,
      body: 'Bad Gateway',
    })

    const request = fetchSystemHealth(createTestApiClient(backend))

    await expect(request).rejects.toThrow(ApiRequestError)
    await expect(request).rejects.toThrow('GET /health respondeu 502')
  })
})
