import type { ApiClient } from '@/api/apiClient'
import { ApiRequestError } from '@/api/ApiRequestError'
import type { LedgerAccountGroup } from './chartOfAccounts'

const CHART_PATH = '/v1/ledger-accounts'

/**
 * Plano de Contas completo: grupos da DRE com suas contas, na ordem dos códigos.
 * @example const groups = await fetchChartOfAccounts(client, signal)
 */
export async function fetchChartOfAccounts(
  client: ApiClient,
  signal?: AbortSignal,
): Promise<LedgerAccountGroup[]> {
  const { data, response } = await client.GET(CHART_PATH, signal ? { signal } : {})
  if (data?.data) return data.data

  throw new ApiRequestError({
    method: 'GET',
    path: CHART_PATH,
    status: response.status,
    expected: 'ResponseOfListOfLedgerAccountGroup (200)',
  })
}
