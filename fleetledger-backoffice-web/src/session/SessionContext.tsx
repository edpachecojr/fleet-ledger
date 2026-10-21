import { createContext, useCallback, useContext, useSyncExternalStore, type ReactNode } from 'react'
import type { AdminSession } from './AdminSession'

const SessionContext = createContext<AdminSession | null>(null)

interface SessionProviderProps {
  session: AdminSession
  children: ReactNode
}

// Injeção da sessão: a aplicação passa a real (sessionStorage); os testes, uma com storage e relógio falsos.
export function SessionProvider({ session, children }: SessionProviderProps): ReactNode {
  return <SessionContext value={session}>{children}</SessionContext>
}

export function useAdminSession(): AdminSession {
  const session = useContext(SessionContext)
  if (!session)
    throw new Error('useAdminSession chamado fora de <SessionProvider>. Envolva a árvore com o provider.')
  return session
}

/** Verdadeiro enquanto há token válido; re-renderiza quando a sessão começa ou termina. */
export function useIsSessionActive(): boolean {
  const session = useAdminSession()
  const subscribe = useCallback((listener: () => void) => session.subscribe(listener), [session])
  return useSyncExternalStore(subscribe, () => session.isActive())
}
