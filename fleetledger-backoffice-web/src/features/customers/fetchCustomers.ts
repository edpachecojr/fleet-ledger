import type { ApiClient } from '@/api/apiClient'
import { ApiRequestError } from '@/api/ApiRequestError'
import { listQueryParams, type ListQuery } from '@/api/listQuery'
import type { CustomerPage } from './customer'

const CUSTOMERS_PATH = '/v1/customers'

/**
 * Página de clientes com a busca e a situação escolhidas.
 * @example const page = await fetchCustomers(client, { search: 'agro', activity: 'active', page: 1 })
 */
export async function fetchCustomers(
  client: ApiClient,
  query: ListQuery,
  signal?: AbortSignal,
): Promise<CustomerPage> {
  const { data, response } = await client.GET(CUSTOMERS_PATH, {
    params: { query: listQueryParams(query) },
    ...(signal ? { signal } : {}),
  })
  if (data) return data

  throw new ApiRequestError({
    method: 'GET',
    path: CUSTOMERS_PATH,
    status: response.status,
    expected: 'PagedResponseOfListOfCustomer (200)',
  })
}
