import type { ApiClient } from '@/api/apiClient'
import { ApiRequestError } from '@/api/ApiRequestError'
import { listQueryParams } from '@/api/listQuery'
import type { VehicleListQuery, VehiclePage } from './vehicle'

const VEHICLES_PATH = '/v1/vehicles'

/**
 * Página de veículos com a busca, a situação e o status escolhidos.
 * @example const page = await fetchVehicles(client, { search: 'volvo', activity: 'active', status: 'all', page: 1 })
 */
export async function fetchVehicles(
  client: ApiClient,
  query: VehicleListQuery,
  signal?: AbortSignal,
): Promise<VehiclePage> {
  const { data, response } = await client.GET(VEHICLES_PATH, {
    params: {
      query: { ...listQueryParams(query), ...(query.status === 'all' ? {} : { status: query.status }) },
    },
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
