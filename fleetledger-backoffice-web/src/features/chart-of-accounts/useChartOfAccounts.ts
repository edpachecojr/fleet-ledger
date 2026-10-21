import { useQuery, type UseQueryResult } from '@tanstack/react-query'
import { useApiClient } from '@/api/ApiClientContext'
import type { LedgerAccountGroup } from './chartOfAccounts'
import { fetchChartOfAccounts } from './fetchChartOfAccounts'

// O plano é fixo (só muda com nova versão da API): não precisa revalidar durante a sessão.
export function useChartOfAccounts(): UseQueryResult<LedgerAccountGroup[]> {
  const client = useApiClient()

  return useQuery({
    queryKey: ['chart-of-accounts'],
    queryFn: ({ signal }) => fetchChartOfAccounts(client, signal),
    staleTime: Infinity,
  })
}
