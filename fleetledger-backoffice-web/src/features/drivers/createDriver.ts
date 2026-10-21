import type { ApiClient } from '@/api/apiClient'
import { outcomeOf, type MutationOutcome } from '@/api/mutationOutcome'
import type { CreateDriverRequest, Driver } from './driver'

/**
 * Cadastra o motorista; CPF ou CNH duplicados (409) e campos inválidos (400) voltam como recusa.
 * @example const outcome = await createDriver(client, request)
 */
export async function createDriver(
  client: ApiClient,
  request: CreateDriverRequest,
): Promise<MutationOutcome<Driver>> {
  return outcomeOf(await client.POST('/v1/drivers', { body: request }), {
    method: 'POST',
    path: '/v1/drivers',
    expected: 'ResponseOfDriver (201, 400 ou 409)',
  })
}
