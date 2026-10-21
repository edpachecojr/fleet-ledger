import { keepPreviousData, useQuery, type UseQueryResult } from '@tanstack/react-query'
import { useApiClient } from '@/api/ApiClientContext'
import type { Driver, DriverListQuery, DriverPage } from './driver'
import { fetchDriver } from './fetchDriver'
import { fetchDrivers } from './fetchDrivers'

// Prefixo comum: qualquer escrita invalida lista e detalhe de uma vez.
export const DRIVERS_QUERY_KEY = ['drivers'] as const

export function useDrivers(query: DriverListQuery): UseQueryResult<DriverPage> {
  const client = useApiClient()

  return useQuery({
    queryKey: [...DRIVERS_QUERY_KEY, 'list', query],
    queryFn: ({ signal }) => fetchDrivers(client, query, signal),
    placeholderData: keepPreviousData,
  })
}

export function useDriver(driverId: string): UseQueryResult<Driver | null> {
  const client = useApiClient()

  return useQuery({
    queryKey: [...DRIVERS_QUERY_KEY, 'detail', driverId],
    queryFn: ({ signal }) => fetchDriver(client, driverId, signal),
  })
}
