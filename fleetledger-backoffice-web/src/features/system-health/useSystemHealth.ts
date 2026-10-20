import { useQuery, type UseQueryResult } from '@tanstack/react-query'
import { useApiClient } from '@/api/ApiClientContext'
import { fetchSystemHealth } from './fetchSystemHealth'
import type { SystemHealth } from './systemHealth'

const REFRESH_INTERVAL_MS = 30_000

export const SYSTEM_HEALTH_QUERY_KEY = ['system-health'] as const

export function useSystemHealth(): UseQueryResult<SystemHealth> {
  const client = useApiClient()

  return useQuery({
    queryKey: SYSTEM_HEALTH_QUERY_KEY,
    queryFn: ({ signal }) => fetchSystemHealth(client, signal),
    refetchInterval: REFRESH_INTERVAL_MS,
  })
}
