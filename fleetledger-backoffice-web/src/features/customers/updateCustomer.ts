import type { ApiClient } from '@/api/apiClient'
import { outcomeOf, type MutationOutcome } from '@/api/mutationOutcome'
import type { Customer, UpdateCustomerRequest } from './customer'

/** Altera tudo menos o CNPJ; cliente de outra organização volta como 404. */
export async function updateCustomer(
  client: ApiClient,
  { customerId, request }: { customerId: string; request: UpdateCustomerRequest },
): Promise<MutationOutcome<Customer>> {
  return outcomeOf(
    await client.PUT('/v1/customers/{id}', { params: { path: { id: customerId } }, body: request }),
    {
      method: 'PUT',
      path: '/v1/customers/{id}',
      expected: 'ResponseOfCustomer (200, 400 ou 404)',
    },
  )
}
