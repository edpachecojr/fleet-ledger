import { useQueryClient } from '@tanstack/react-query'
import { useEffect, type ReactNode } from 'react'
import { Navigate, useLocation } from 'react-router'
import { RoutePath } from '@/app/routePaths'
import { useAdminSession, useIsSessionActive } from './SessionContext'
import { useSessionExpiry } from './useSessionExpiry'

/** Estado de navegação que o login lê para voltar à tela pedida. */
export interface LoginRedirectState {
  from: string
}

interface RequireSessionProps {
  children: ReactNode
}

/** Protege as telas do backoffice: sem sessão válida, manda para o login lembrando a rota pedida. */
export function RequireSession({ children }: RequireSessionProps): ReactNode {
  const session = useAdminSession()
  const isActive = useIsSessionActive()
  const { pathname } = useLocation()
  useSessionExpiry(session, isActive)
  useClearCachedDataWhenSignedOut(isActive)

  if (!isActive) {
    const state: LoginRedirectState = { from: pathname }
    return <Navigate to={RoutePath.login} replace state={state} />
  }
  return children
}

// Dado da sessão anterior não pode aparecer para quem fizer o próximo login.
function useClearCachedDataWhenSignedOut(isActive: boolean): void {
  const queryClient = useQueryClient()
  useEffect(() => {
    if (!isActive) queryClient.clear()
  }, [isActive, queryClient])
}
