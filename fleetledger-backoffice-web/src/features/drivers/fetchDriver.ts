import type { ApiClient } from '@/api/apiClient'
import { ApiRequestError } from '@/api/ApiRequestError'
import type { Driver } from './driver'

const DRIVER_PATH = '/v1/drivers/{id}'
const NOT_FOUND_STATUS = 404

/**
 * Um motorista pelo id; `null` quando ele não existe na organização (404).
 * @example const driver = await fetchDriver(client, driverId)
 */
export async function fetchDriver(
  client: ApiClient,
  driverId: string,
  signal?: AbortSignal,
): Promise<Driver | null> {
  const { data, response } = await client.GET(DRIVER_PATH, {
    params: { path: { id: driverId } },
    ...(signal ? { signal } : {}),
  })
  if (data?.data) return data.data
  if (response.status === NOT_FOUND_STATUS) return null

  throw new ApiRequestError({
    method: 'GET',
    path: DRIVER_PATH,
    status: response.status,
    expected: 'ResponseOfDriver (200 ou 404)',
  })
}
