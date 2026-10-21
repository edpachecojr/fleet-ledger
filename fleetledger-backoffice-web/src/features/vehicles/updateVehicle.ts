import type { ApiClient } from '@/api/apiClient'
import { outcomeOf, type MutationOutcome } from '@/api/mutationOutcome'
import type { UpdateVehicleRequest, Vehicle } from './vehicle'

/** Altera modelo, capacidade e taxa; veículo de outra organização volta como 404. */
export async function updateVehicle(
  client: ApiClient,
  { vehicleId, request }: { vehicleId: string; request: UpdateVehicleRequest },
): Promise<MutationOutcome<Vehicle>> {
  return outcomeOf(
    await client.PUT('/v1/vehicles/{id}', { params: { path: { id: vehicleId } }, body: request }),
    {
      method: 'PUT',
      path: '/v1/vehicles/{id}',
      expected: 'ResponseOfVehicle (200, 400 ou 404)',
    },
  )
}
