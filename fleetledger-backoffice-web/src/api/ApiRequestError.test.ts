import { ApiRequestError } from './ApiRequestError'

describe('ApiRequestError', () => {
  it('names method, path, received status and expected shape in the message', () => {
    const error = new ApiRequestError({ method: 'GET', path: '/health', status: 502, expected: 'JSON' })

    expect(error.message).toBe('GET /health respondeu 502. Esperado: JSON.')
    expect(error.details.status).toBe(502)
  })
})
