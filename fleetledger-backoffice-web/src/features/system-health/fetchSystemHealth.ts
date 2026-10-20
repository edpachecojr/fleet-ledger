import type { ApiClient } from '@/api/apiClient'
import { ApiRequestError } from '@/api/ApiRequestError'
import { isSystemHealth, type SystemHealth } from './systemHealth'

const HEALTH_PATH = '/health'

/**
 * Consulta a saúde da API. 503 não é erro aqui: o corpo diz qual dependência caiu.
 * @example const health = await fetchSystemHealth(client, signal)
 */
export async function fetchSystemHealth(client: ApiClient, signal?: AbortSignal): Promise<SystemHealth> {
  const { data, error, response } = await client.GET(HEALTH_PATH, signal ? { signal } : {})
  const report: unknown = data ?? error
  if (isSystemHealth(report)) return report

  throw new ApiRequestError({
    method: 'GET',
    path: HEALTH_PATH,
    status: response.status,
    expected: 'HealthCheckResponse em JSON (200 ou 503)',
  })
}
