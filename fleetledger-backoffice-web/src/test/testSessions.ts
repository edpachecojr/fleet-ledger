import { AdminSession } from '@/session/AdminSession'
import type { IssuedAccessToken } from '@/session/storedSession'
import { FakeClock } from './fakes/FakeClock'
import { FakeSessionStorage } from './fakes/FakeSessionStorage'

export const EIGHT_HOURS_MS = 8 * 60 * 60 * 1000

// Emitido às 20:00 do relógio falso, vence 8 h depois (mesma vida útil da API).
export const TEST_ACCESS_TOKEN: IssuedAccessToken = {
  accessToken: 'test-access-token',
  expiresAt: '2026-10-21T04:00:00Z',
}

export interface TestSession {
  session: AdminSession
  clock: FakeClock
}

export function signedOutSession(): TestSession {
  const clock = new FakeClock()
  return { session: new AdminSession({ storage: new FakeSessionStorage(), now: clock.now }), clock }
}

export function signedInSession(): TestSession {
  const signedOut = signedOutSession()
  signedOut.session.start(TEST_ACCESS_TOKEN)
  return signedOut
}
