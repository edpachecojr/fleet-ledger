import { parseStoredSession } from './storedSession'

describe('parseStoredSession', () => {
  it('returns the token saved by the session', () => {
    const raw = JSON.stringify({ accessToken: 'jwt', expiresAt: '2026-10-21T04:00:00Z' })

    expect(parseStoredSession(raw)).toEqual({ accessToken: 'jwt', expiresAt: '2026-10-21T04:00:00Z' })
  })

  it.each([
    ['nothing saved', null],
    ['invalid JSON', '{not json'],
    ['missing expiration', JSON.stringify({ accessToken: 'jwt' })],
    ['unparseable expiration', JSON.stringify({ accessToken: 'jwt', expiresAt: 'amanhã' })],
    ['token that is not text', JSON.stringify({ accessToken: 42, expiresAt: '2026-10-21T04:00:00Z' })],
  ])('treats %s as no session', (_, raw) => {
    expect(parseStoredSession(raw)).toBeNull()
  })
})
