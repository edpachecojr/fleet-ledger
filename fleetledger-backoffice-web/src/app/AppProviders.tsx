import { QueryClientProvider, type QueryClient } from '@tanstack/react-query'
import type { ReactNode } from 'react'
import type { ApiClient } from '@/api/apiClient'
import { ApiClientProvider } from '@/api/ApiClientContext'

interface AppProvidersProps {
  apiClient: ApiClient
  queryClient: QueryClient
  children: ReactNode
}

// Dependências entram por props: a aplicação passa as reais, os testes passam fakes.
export function AppProviders({ apiClient, queryClient, children }: AppProvidersProps): ReactNode {
  return (
    <QueryClientProvider client={queryClient}>
      <ApiClientProvider client={apiClient}>{children}</ApiClientProvider>
    </QueryClientProvider>
  )
}
