import type { ApiClient } from '@/api/apiClient'
import { ApiRequestError } from '@/api/ApiRequestError'
import type { Customer } from './customer'

const CUSTOMER_PATH = '/v1/customers/{id}'
const NOT_FOUND_STATUS = 404

/**
 * Um cliente pelo id; `null` quando ele não existe na organização (404).
 * @example const customer = await fetchCustomer(client, customerId)
 */
export async function fetchCustomer(
  client: ApiClient,
  customerId: string,
  signal?: AbortSignal,
): Promise<Customer | null> {
  const { data, response } = await client.GET(CUSTOMER_PATH, {
    params: { path: { id: customerId } },
    ...(signal ? { signal } : {}),
  })
  if (data?.data) return data.data
  if (response.status === NOT_FOUND_STATUS) return null

  throw new ApiRequestError({
    method: 'GET',
    path: CUSTOMER_PATH,
    status: response.status,
    expected: 'ResponseOfCustomer (200 ou 404)',
  })
}
