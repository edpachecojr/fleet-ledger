import { EIGHT_HOURS_MS, signedInSession, signedOutSession, TEST_ACCESS_TOKEN } from '@/test/testSessions'

describe('AdminSession', () => {
  it('has no access token before login', () => {
    const { session } = signedOutSession()

    expect(session.getAccessToken()).toBeNull()
  })

  it('serves the issued access token after start', () => {
    const { session } = signedInSession()

    expect(session.getAccessToken()).toBe(TEST_ACCESS_TOKEN.accessToken)
  })

  it('stops being active once the token expires', () => {
    const { session, clock } = signedInSession()

    clock.advance(EIGHT_HOURS_MS)

    expect(session.isActive()).toBe(false)
  })

  it('reports the time left until expiry', () => {
    const { session, clock } = signedInSession()

    clock.advance(1000)

    expect(session.millisecondsUntilExpiry()).toBe(EIGHT_HOURS_MS - 1000)
  })

  it('forgets the token on end', () => {
    const { session } = signedInSession()

    session.end()

    expect(session.getAccessToken()).toBeNull()
  })

  it('notifies subscribers when the session starts and ends', () => {
    const { session } = signedOutSession()
    const listener = vi.fn()
    session.subscribe(listener)

    session.start(TEST_ACCESS_TOKEN)
    session.end()

    expect(listener).toHaveBeenCalledTimes(2)
  })

  it('does not notify when ending a session that does not exist', () => {
    const { session } = signedOutSession()
    const listener = vi.fn()
    session.subscribe(listener)

    session.end()

    expect(listener).not.toHaveBeenCalled()
  })

  it('ends the session when the API discards the token', () => {
    const { session } = signedInSession()

    session.discardAccessToken()

    expect(session.isActive()).toBe(false)
  })
})
