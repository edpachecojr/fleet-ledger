import { useEffect } from 'react'
import type { AdminSession } from './AdminSession'

/**
 * Encerra a sessão no instante em que o token vence, para a tela voltar ao login
 * sem esperar a próxima chamada à API responder 401.
 * @example useSessionExpiry(session, isActive)
 */
export function useSessionExpiry(session: AdminSession, isActive: boolean): void {
  useEffect(() => {
    const remaining = session.millisecondsUntilExpiry()
    if (!isActive || remaining === null) return

    const timer = setTimeout(() => {
      session.end()
    }, remaining)
    return () => {
      clearTimeout(timer)
    }
  }, [session, isActive])
}
