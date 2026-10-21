import type { ApiClient } from '@/api/apiClient'
import { outcomeOf, type MutationOutcome } from '@/api/mutationOutcome'
import type { Driver, UpdateDriverRequest } from './driver'

/** Altera tudo menos o CPF; CNH de outro motorista volta como 409. */
export async function updateDriver(
  client: ApiClient,
  { driverId, request }: { driverId: string; request: UpdateDriverRequest },
): Promise<MutationOutcome<Driver>> {
  return outcomeOf(
    await client.PUT('/v1/drivers/{id}', { params: { path: { id: driverId } }, body: request }),
    {
      method: 'PUT',
      path: '/v1/drivers/{id}',
      expected: 'ResponseOfDriver (200, 400, 404 ou 409)',
    },
  )
}
