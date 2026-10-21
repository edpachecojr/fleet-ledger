import type { components } from '@/api/generated/schema'

export type IssuedAccessToken = components['schemas']['IssuedAccessToken']

export const SESSION_STORAGE_KEY = 'fleetledger-session'

/** Subconjunto de `Storage` usado pela sessão; os testes passam um fake em memória. */
export interface SessionStorageLike {
  getItem: (key: string) => string | null
  setItem: (key: string, value: string) => void
  removeItem: (key: string) => void
}

/**
 * Lê o token salvo, descartando conteúdo adulterado ou de outra versão do app.
 * @example parseStoredSession('{"accessToken":"eyJ...","expiresAt":"2026-10-21T04:00:00Z"}')
 */
export function parseStoredSession(raw: string | null): IssuedAccessToken | null {
  if (raw === null) return null
  try {
    const candidate: unknown = JSON.parse(raw)
    return isIssuedAccessToken(candidate) ? candidate : null
  } catch (error) {
    if (error instanceof SyntaxError) return null
    throw error
  }
}

function isIssuedAccessToken(candidate: unknown): candidate is IssuedAccessToken {
  if (typeof candidate !== 'object' || candidate === null) return false
  if (!('accessToken' in candidate) || !('expiresAt' in candidate)) return false
  const { accessToken, expiresAt } = candidate
  return (
    typeof accessToken === 'string' && typeof expiresAt === 'string' && !Number.isNaN(Date.parse(expiresAt))
  )
}
