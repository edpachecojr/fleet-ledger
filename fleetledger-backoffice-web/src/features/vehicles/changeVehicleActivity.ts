import type { ApiClient } from '@/api/apiClient'
import { outcomeOf, type MutationOutcome } from '@/api/mutationOutcome'
import type { Vehicle } from './vehicle'

export type VehicleActivityAction = 'deactivate' | 'reactivate'

/** Inativa ou reativa o veículo; a API mantém o status operacional nos dois casos. */
export async function changeVehicleActivity(
  client: ApiClient,
  { vehicleId, action }: { vehicleId: string; action: VehicleActivityAction },
): Promise<MutationOutcome<Vehicle>> {
  const path = action === 'deactivate' ? '/v1/vehicles/{id}/deactivate' : '/v1/vehicles/{id}/reactivate'
  return outcomeOf(await client.POST(path, { params: { path: { id: vehicleId } } }), {
    method: 'POST',
    path,
    expected: 'ResponseOfVehicle (200 ou 404)',
  })
}
