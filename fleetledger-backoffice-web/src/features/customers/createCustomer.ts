import type { ApiClient } from '@/api/apiClient'
import { outcomeOf, type MutationOutcome } from '@/api/mutationOutcome'
import type { CreateCustomerRequest, Customer } from './customer'

/**
 * Cadastra o cliente; CNPJ duplicado (409) e campos inválidos (400) voltam como recusa.
 * @example const outcome = await createCustomer(client, request)
 */
export async function createCustomer(
  client: ApiClient,
  request: CreateCustomerRequest,
): Promise<MutationOutcome<Customer>> {
  return outcomeOf(await client.POST('/v1/customers', { body: request }), {
    method: 'POST',
    path: '/v1/customers',
    expected: 'ResponseOfCustomer (201, 400 ou 409)',
  })
}
