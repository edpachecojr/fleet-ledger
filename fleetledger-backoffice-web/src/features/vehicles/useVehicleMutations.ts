import { useMutation, useQueryClient, type UseMutationResult } from '@tanstack/react-query'
import { useApiClient } from '@/api/ApiClientContext'
import type { MutationOutcome } from '@/api/mutationOutcome'
import { changeVehicleActivity, type VehicleActivityAction } from './changeVehicleActivity'
import { changeVehicleStatus, type ManualVehicleStatus } from './changeVehicleStatus'
import { createVehicle } from './createVehicle'
import { updateVehicle } from './updateVehicle'
import { VEHICLES_QUERY_KEY } from './useVehicles'
import type { CreateVehicleRequest, UpdateVehicleRequest, Vehicle } from './vehicle'

type VehicleMutation<TVariables> = UseMutationResult<MutationOutcome<Vehicle>, Error, TVariables>

export function useCreateVehicle(): VehicleMutation<CreateVehicleRequest> {
  const client = useApiClient()
  return useMutation({
    mutationFn: (request: CreateVehicleRequest) => createVehicle(client, request),
    onSuccess: useInvalidateVehiclesOnSave(),
  })
}

export function useUpdateVehicle(vehicleId: string): VehicleMutation<UpdateVehicleRequest> {
  const client = useApiClient()
  return useMutation({
    mutationFn: (request: UpdateVehicleRequest) => updateVehicle(client, { vehicleId, request }),
    onSuccess: useInvalidateVehiclesOnSave(),
  })
}

export function useChangeVehicleActivity(vehicleId: string): VehicleMutation<VehicleActivityAction> {
  const client = useApiClient()
  return useMutation({
    mutationFn: (action: VehicleActivityAction) => changeVehicleActivity(client, { vehicleId, action }),
    onSuccess: useInvalidateVehiclesOnSave(),
  })
}

export function useChangeVehicleStatus(vehicleId: string): VehicleMutation<ManualVehicleStatus> {
  const client = useApiClient()
  return useMutation({
    mutationFn: (status: ManualVehicleStatus) => changeVehicleStatus(client, { vehicleId, status }),
    onSuccess: useInvalidateVehiclesOnSave(),
  })
}

function useInvalidateVehiclesOnSave(): (outcome: MutationOutcome<Vehicle>) => Promise<void> {
  const queryClient = useQueryClient()
  return async (outcome) => {
    if (outcome.kind === 'saved') await queryClient.invalidateQueries({ queryKey: VEHICLES_QUERY_KEY })
  }
}
