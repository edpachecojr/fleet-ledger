import { keepPreviousData, useQuery, type UseQueryResult } from '@tanstack/react-query'
import { useApiClient } from '@/api/ApiClientContext'
import { fetchVehicle } from './fetchVehicle'
import { fetchVehicles } from './fetchVehicles'
import type { Vehicle, VehicleListQuery, VehiclePage } from './vehicle'

// Prefixo comum: qualquer escrita invalida lista e detalhe de uma vez.
export const VEHICLES_QUERY_KEY = ['vehicles'] as const

// A página anterior fica na tela enquanto a próxima busca carrega: a tabela não pisca.
export function useVehicles(query: VehicleListQuery): UseQueryResult<VehiclePage> {
  const client = useApiClient()

  return useQuery({
    queryKey: [...VEHICLES_QUERY_KEY, 'list', query],
    queryFn: ({ signal }) => fetchVehicles(client, query, signal),
    placeholderData: keepPreviousData,
  })
}

export function useVehicle(vehicleId: string): UseQueryResult<Vehicle | null> {
  const client = useApiClient()

  return useQuery({
    queryKey: [...VEHICLES_QUERY_KEY, 'detail', vehicleId],
    queryFn: ({ signal }) => fetchVehicle(client, vehicleId, signal),
  })
}
