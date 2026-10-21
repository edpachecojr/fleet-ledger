import { QueryClientProvider, type QueryClient } from '@tanstack/react-query'
import type { ReactNode } from 'react'
import type { ApiClient } from '@/api/apiClient'
import { ApiClientProvider } from '@/api/ApiClientContext'
import type { AdminSession } from '@/session/AdminSession'
import { SessionProvider } from '@/session/SessionContext'

interface AppProvidersProps {
  apiClient: ApiClient
  queryClient: QueryClient
  session: AdminSession
  children: ReactNode
}

// Dependências entram por props: a aplicação passa as reais, os testes passam fakes.
export function AppProviders({ apiClient, queryClient, session, children }: AppProvidersProps): ReactNode {
  return (
    <QueryClientProvider client={queryClient}>
      <SessionProvider session={session}>
        <ApiClientProvider client={apiClient}>{children}</ApiClientProvider>
      </SessionProvider>
    </QueryClientProvider>
  )
}
