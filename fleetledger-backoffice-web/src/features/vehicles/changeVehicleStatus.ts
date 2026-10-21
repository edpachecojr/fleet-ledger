import type { ApiClient } from '@/api/apiClient'
import { outcomeOf, type MutationOutcome } from '@/api/mutationOutcome'
import type { Vehicle } from './vehicle'

/** Status que o administrador escolhe; "Em viagem" é só do sistema (TASK-005). */
export type ManualVehicleStatus = 'Available' | 'InMaintenance'

/**
 * Troca manual do status; veículo em viagem ou inativo volta como recusa (409) com a mensagem da API.
 * @example await changeVehicleStatus(client, { vehicleId, status: 'InMaintenance' })
 */
export async function changeVehicleStatus(
  client: ApiClient,
  { vehicleId, status }: { vehicleId: string; status: ManualVehicleStatus },
): Promise<MutationOutcome<Vehicle>> {
  const path = '/v1/vehicles/{id}/status'
  return outcomeOf(await client.PUT(path, { params: { path: { id: vehicleId } }, body: { status } }), {
    method: 'PUT',
    path,
    expected: 'ResponseOfVehicle (200, 400, 404 ou 409)',
  })
}
