import type { ApiClient } from '@/api/apiClient'
import { ApiRequestError } from '@/api/ApiRequestError'
import { listQueryParams, type ListQuery } from '@/api/listQuery'
import type { VehiclePage } from './vehicle'

const VEHICLES_PATH = '/v1/vehicles'

/**
 * Página de veículos com a busca e a situação escolhidas.
 * @example const page = await fetchVehicles(client, { search: 'volvo', activity: 'active', page: 1 })
 */
export async function fetchVehicles(
  client: ApiClient,
  query: ListQuery,
  signal?: AbortSignal,
): Promise<VehiclePage> {
  const { data, response } = await client.GET(VEHICLES_PATH, {
    params: { query: listQueryParams(query) },
    ...(signal ? { signal } : {}),
  })
  if (data) return data

  throw new ApiRequestError({
    method: 'GET',
    path: VEHICLES_PATH,
    status: response.status,
    expected: 'PagedResponseOfListOfVehicle (200)',
  })
}
