import { useMutation, useQueryClient, type UseMutationResult } from '@tanstack/react-query'
import { useApiClient } from '@/api/ApiClientContext'
import type { MutationOutcome } from '@/api/mutationOutcome'
import { changeDriverActivity } from './changeDriverActivity'
import { createDriver } from './createDriver'
import type { CreateDriverRequest, Driver, UpdateDriverRequest } from './driver'
import { updateDriver } from './updateDriver'
import { DRIVERS_QUERY_KEY } from './useDrivers'

type DriverMutation<TVariables> = UseMutationResult<MutationOutcome<Driver>, Error, TVariables>

export function useCreateDriver(): DriverMutation<CreateDriverRequest> {
  const client = useApiClient()
  return useMutation({
    mutationFn: (request: CreateDriverRequest) => createDriver(client, request),
    onSuccess: useInvalidateDriversOnSave(),
  })
}

export function useUpdateDriver(driverId: string): DriverMutation<UpdateDriverRequest> {
  const client = useApiClient()
  return useMutation({
    mutationFn: (request: UpdateDriverRequest) => updateDriver(client, { driverId, request }),
    onSuccess: useInvalidateDriversOnSave(),
  })
}

export function useChangeDriverActivity(driverId: string): DriverMutation<boolean> {
  const client = useApiClient()
  return useMutation({
    mutationFn: (deactivate: boolean) => changeDriverActivity(client, { driverId, deactivate }),
    onSuccess: useInvalidateDriversOnSave(),
  })
}

function useInvalidateDriversOnSave(): (outcome: MutationOutcome<Driver>) => Promise<void> {
  const queryClient = useQueryClient()
  return async (outcome) => {
    if (outcome.kind === 'saved') await queryClient.invalidateQueries({ queryKey: DRIVERS_QUERY_KEY })
  }
}
