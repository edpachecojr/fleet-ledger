import { keepPreviousData, useQuery, type UseQueryResult } from '@tanstack/react-query'
import { useApiClient } from '@/api/ApiClientContext'
import type { ListQuery } from '@/api/listQuery'
import type { Customer, CustomerPage } from './customer'
import { fetchCustomer } from './fetchCustomer'
import { fetchCustomers } from './fetchCustomers'

// Prefixo comum: qualquer escrita invalida lista e detalhe de uma vez.
export const CUSTOMERS_QUERY_KEY = ['customers'] as const

export function useCustomers(query: ListQuery): UseQueryResult<CustomerPage> {
  const client = useApiClient()

  return useQuery({
    queryKey: [...CUSTOMERS_QUERY_KEY, 'list', query],
    queryFn: ({ signal }) => fetchCustomers(client, query, signal),
    placeholderData: keepPreviousData,
  })
}

export function useCustomer(customerId: string): UseQueryResult<Customer | null> {
  const client = useApiClient()

  return useQuery({
    queryKey: [...CUSTOMERS_QUERY_KEY, 'detail', customerId],
    queryFn: ({ signal }) => fetchCustomer(client, customerId, signal),
  })
}
