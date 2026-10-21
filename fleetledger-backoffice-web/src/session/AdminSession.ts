import type { AccessTokenSource } from '@/api/accessTokenSource'
import {
  parseStoredSession,
  SESSION_STORAGE_KEY,
  type IssuedAccessToken,
  type SessionStorageLike,
} from './storedSession'

export interface AdminSessionDependencies {
  storage: SessionStorageLike
  now: () => number
}

type SessionListener = () => void

/**
 * Sessão do administrador: guarda o access token (sessionStorage, decisão da TASK-002)
 * e avisa quem assina quando ela começa ou termina. Token vencido conta como sem sessão.
 * @example const session = new AdminSession({ storage: window.sessionStorage, now: () => Date.now() })
 */
export class AdminSession implements AccessTokenSource {
  private readonly dependencies: AdminSessionDependencies
  private readonly listeners = new Set<SessionListener>()

  constructor(dependencies: AdminSessionDependencies) {
    this.dependencies = dependencies
  }

  start(issued: IssuedAccessToken): void {
    this.dependencies.storage.setItem(SESSION_STORAGE_KEY, JSON.stringify(issued))
    this.notify()
  }

  end(): void {
    if (this.dependencies.storage.getItem(SESSION_STORAGE_KEY) === null) return
    this.dependencies.storage.removeItem(SESSION_STORAGE_KEY)
    this.notify()
  }

  isActive(): boolean {
    return this.validToken() !== null
  }

  /** Milissegundos até o token vencer; null sem sessão. */
  millisecondsUntilExpiry(): number | null {
    const token = this.validToken()
    return token ? Date.parse(token.expiresAt) - this.dependencies.now() : null
  }

  getAccessToken(): string | null {
    return this.validToken()?.accessToken ?? null
  }

  discardAccessToken(): void {
    this.end()
  }

  /** Assinatura no formato de `useSyncExternalStore`. */
  subscribe(listener: SessionListener): () => void {
    this.listeners.add(listener)
    return () => this.listeners.delete(listener)
  }

  private validToken(): IssuedAccessToken | null {
    const token = parseStoredSession(this.dependencies.storage.getItem(SESSION_STORAGE_KEY))
    if (!token) return null
    return Date.parse(token.expiresAt) > this.dependencies.now() ? token : null
  }

  private notify(): void {
    this.listeners.forEach((listener) => {
      listener()
    })
  }
}
