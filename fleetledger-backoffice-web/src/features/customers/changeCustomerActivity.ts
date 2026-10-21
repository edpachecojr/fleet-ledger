import type { ApiClient } from '@/api/apiClient'
import { outcomeOf, type MutationOutcome } from '@/api/mutationOutcome'
import type { Customer } from './customer'

/** Inativa (some da abertura de viagem) ou reativa o cliente. */
export async function changeCustomerActivity(
  client: ApiClient,
  { customerId, deactivate }: { customerId: string; deactivate: boolean },
): Promise<MutationOutcome<Customer>> {
  const path = deactivate ? '/v1/customers/{id}/deactivate' : '/v1/customers/{id}/reactivate'
  return outcomeOf(await client.POST(path, { params: { path: { id: customerId } } }), {
    method: 'POST',
    path,
    expected: 'ResponseOfCustomer (200 ou 404)',
  })
}
