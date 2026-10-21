import { useMutation, useQueryClient, type UseMutationResult } from '@tanstack/react-query'
import { useApiClient } from '@/api/ApiClientContext'
import type { MutationOutcome } from '@/api/mutationOutcome'
import { changeCustomerActivity } from './changeCustomerActivity'
import { createCustomer } from './createCustomer'
import type { CreateCustomerRequest, Customer, UpdateCustomerRequest } from './customer'
import { updateCustomer } from './updateCustomer'
import { CUSTOMERS_QUERY_KEY } from './useCustomers'

type CustomerMutation<TVariables> = UseMutationResult<MutationOutcome<Customer>, Error, TVariables>

export function useCreateCustomer(): CustomerMutation<CreateCustomerRequest> {
  const client = useApiClient()
  return useMutation({
    mutationFn: (request: CreateCustomerRequest) => createCustomer(client, request),
    onSuccess: useInvalidateCustomersOnSave(),
  })
}

export function useUpdateCustomer(customerId: string): CustomerMutation<UpdateCustomerRequest> {
  const client = useApiClient()
  return useMutation({
    mutationFn: (request: UpdateCustomerRequest) => updateCustomer(client, { customerId, request }),
    onSuccess: useInvalidateCustomersOnSave(),
  })
}

export function useChangeCustomerActivity(customerId: string): CustomerMutation<boolean> {
  const client = useApiClient()
  return useMutation({
    mutationFn: (deactivate: boolean) => changeCustomerActivity(client, { customerId, deactivate }),
    onSuccess: useInvalidateCustomersOnSave(),
  })
}

function useInvalidateCustomersOnSave(): (outcome: MutationOutcome<Customer>) => Promise<void> {
  const queryClient = useQueryClient()
  return async (outcome) => {
    if (outcome.kind === 'saved') await queryClient.invalidateQueries({ queryKey: CUSTOMERS_QUERY_KEY })
  }
}
