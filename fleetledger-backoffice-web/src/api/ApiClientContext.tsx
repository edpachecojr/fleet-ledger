import { createContext, useContext, type ReactNode } from 'react'
import type { ApiClient } from './apiClient'

const ApiClientContext = createContext<ApiClient | null>(null)

interface ApiClientProviderProps {
  client: ApiClient
  children: ReactNode
}

// Injeção do cliente HTTP: a aplicação passa o real; os testes, um com fetch falso.
export function ApiClientProvider({ client, children }: ApiClientProviderProps): ReactNode {
  return <ApiClientContext value={client}>{children}</ApiClientContext>
}

export function useApiClient(): ApiClient {
  const client = useContext(ApiClientContext)
  if (!client)
    throw new Error('useApiClient chamado fora de <ApiClientProvider>. Envolva a árvore com o provider.')
  return client
}
