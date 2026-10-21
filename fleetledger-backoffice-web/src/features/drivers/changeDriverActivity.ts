import type { ApiClient } from '@/api/apiClient'
import { outcomeOf, type MutationOutcome } from '@/api/mutationOutcome'
import type { Driver } from './driver'

/** Inativa (some da abertura de viagem) ou reativa o motorista. */
export async function changeDriverActivity(
  client: ApiClient,
  { driverId, deactivate }: { driverId: string; deactivate: boolean },
): Promise<MutationOutcome<Driver>> {
  const path = deactivate ? '/v1/drivers/{id}/deactivate' : '/v1/drivers/{id}/reactivate'
  return outcomeOf(await client.POST(path, { params: { path: { id: driverId } } }), {
    method: 'POST',
    path,
    expected: 'ResponseOfDriver (200 ou 404)',
  })
}
