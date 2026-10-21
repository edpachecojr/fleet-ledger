import type { ApiClient } from '@/api/apiClient'
import { outcomeOf, type MutationOutcome } from '@/api/mutationOutcome'
import type { CreateVehicleRequest, Vehicle } from './vehicle'

/**
 * Cadastra o veículo; placa duplicada (409) e campos inválidos (400) voltam como recusa.
 * @example const outcome = await createVehicle(client, request)
 */
export async function createVehicle(
  client: ApiClient,
  request: CreateVehicleRequest,
): Promise<MutationOutcome<Vehicle>> {
  return outcomeOf(await client.POST('/v1/vehicles', { body: request }), {
    method: 'POST',
    path: '/v1/vehicles',
    expected: 'ResponseOfVehicle (201, 400 ou 409)',
  })
}
