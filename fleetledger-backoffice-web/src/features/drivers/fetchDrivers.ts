import type { ApiClient } from '@/api/apiClient'
import { ApiRequestError } from '@/api/ApiRequestError'
import { listQueryParams } from '@/api/listQuery'
import type { DriverListQuery, DriverPage } from './driver'

const DRIVERS_PATH = '/v1/drivers'

/**
 * Página de motoristas com busca, situação e situação documental escolhidas.
 * @example await fetchDrivers(client, { search: '', activity: 'active', documentStatus: 'Expired', page: 1 })
 */
export async function fetchDrivers(
  client: ApiClient,
  query: DriverListQuery,
  signal?: AbortSignal,
): Promise<DriverPage> {
  const documentStatus = query.documentStatus === 'all' ? {} : { documentStatus: query.documentStatus }
  const { data, response } = await client.GET(DRIVERS_PATH, {
    params: { query: { ...listQueryParams(query), ...documentStatus } },
    ...(signal ? { signal } : {}),
  })
  if (data) return data

  throw new ApiRequestError({
    method: 'GET',
    path: DRIVERS_PATH,
    status: response.status,
    expected: 'PagedResponseOfListOfDriver (200)',
  })
}
