import { ApiRequestError } from '@/api/ApiRequestError'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { createTestApiClient } from '@/test/renderApp'
import { TEST_ACCESS_TOKEN } from '@/test/testSessions'
import {
  FALLBACK_REFUSAL_MESSAGE,
  MISSING_CREDENTIALS_MESSAGE,
  requestAccessToken,
} from './requestAccessToken'

const LOGIN_ROUTE = 'POST /api/v1/auth/login'
const CREDENTIALS = { email: 'admin@fleetledger.local', password: 'Senha-Forte-123' }

describe('requestAccessToken', () => {
  it('returns the access token when the API accepts the credentials', async () => {
    const backend = new FakeHttpBackend().respondWithJson(LOGIN_ROUTE, {
      status: 200,
      body: { data: TEST_ACCESS_TOKEN, message: null, code: 200 },
    })

    const outcome = await requestAccessToken(createTestApiClient(backend), CREDENTIALS)

    expect(outcome).toEqual({ kind: 'granted', accessToken: TEST_ACCESS_TOKEN })
  })

  it('sends the credentials as JSON', async () => {
    const backend = new FakeHttpBackend().respondWithJson(LOGIN_ROUTE, {
      status: 200,
      body: { data: TEST_ACCESS_TOKEN, message: null, code: 200 },
    })

    await requestAccessToken(createTestApiClient(backend), CREDENTIALS)

    expect(await backend.receivedRequests[0]?.json()).toEqual(CREDENTIALS)
  })

  it('returns the API refusal message on 401', async () => {
    const backend = new FakeHttpBackend().respondWithJson(LOGIN_ROUTE, {
      status: 401,
      body: { data: null, message: 'E-mail ou senha inválidos.', code: 401 },
    })

    const outcome = await requestAccessToken(createTestApiClient(backend), CREDENTIALS)

    expect(outcome).toEqual({ kind: 'refused', message: 'E-mail ou senha inválidos.' })
  })

  it('falls back to the generic message when a 401 has no body message', async () => {
    const backend = new FakeHttpBackend().respondWithJson(LOGIN_ROUTE, { status: 401, body: {} })

    const outcome = await requestAccessToken(createTestApiClient(backend), CREDENTIALS)

    expect(outcome).toEqual({ kind: 'refused', message: FALLBACK_REFUSAL_MESSAGE })
  })

  it('asks for both fields when the API rejects the shape (400)', async () => {
    const backend = new FakeHttpBackend().respondWithJson(LOGIN_ROUTE, { status: 400, body: { errors: {} } })

    const outcome = await requestAccessToken(createTestApiClient(backend), { email: '', password: '' })

    expect(outcome).toEqual({ kind: 'refused', message: MISSING_CREDENTIALS_MESSAGE })
  })

  it('throws with context on any other answer', async () => {
    const backend = new FakeHttpBackend().respondWithText(LOGIN_ROUTE, { status: 502, body: 'Bad Gateway' })

    const request = requestAccessToken(createTestApiClient(backend), CREDENTIALS)

    await expect(request).rejects.toThrow(ApiRequestError)
    await expect(request).rejects.toThrow('POST /v1/auth/login respondeu 502')
  })
})
