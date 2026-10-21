import type { ApiClient } from '@/api/apiClient'
import { ApiRequestError } from '@/api/ApiRequestError'
import type { Vehicle } from './vehicle'

const VEHICLE_PATH = '/v1/vehicles/{id}'
const NOT_FOUND_STATUS = 404

/**
 * Um veículo pelo id; `null` quando ele não existe na organização (404).
 * @example const vehicle = await fetchVehicle(client, vehicleId)
 */
export async function fetchVehicle(
  client: ApiClient,
  vehicleId: string,
  signal?: AbortSignal,
): Promise<Vehicle | null> {
  const { data, response } = await client.GET(VEHICLE_PATH, {
    params: { path: { id: vehicleId } },
    ...(signal ? { signal } : {}),
  })
  if (data?.data) return data.data
  if (response.status === NOT_FOUND_STATUS) return null

  throw new ApiRequestError({
    method: 'GET',
    path: VEHICLE_PATH,
    status: response.status,
    expected: 'ResponseOfVehicle (200 ou 404)',
  })
}
