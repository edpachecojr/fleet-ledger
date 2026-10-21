import type { ReactNode } from 'react'
import { ActivityToggle } from '@/design-system/components/ActivityToggle'
import { useChangeVehicleActivity } from './useVehicleMutations'
import type { Vehicle } from './vehicle'

/** Inativar (com confirmação) ou reativar o veículo, mantendo o status operacional. */
export function VehicleActivityAction({ vehicle }: { vehicle: Vehicle }): ReactNode {
  const activity = useChangeVehicleActivity(vehicle.id ?? '')

  return (
    <ActivityToggle
      isActive={vehicle.isActive !== false}
      confirmTitle="Inativar veículo?"
      consequence={<DeactivationWarning plate={vehicle.plate ?? ''} />}
      pending={activity.isPending}
      rejection={activity.data?.kind === 'rejected' ? activity.data.message : null}
      onChange={(deactivate, done) => {
        activity.mutate(deactivate ? 'deactivate' : 'reactivate', { onSettled: done })
      }}
    />
  )
}

function DeactivationWarning({ plate }: { plate: string }): ReactNode {
  return (
    <>
      O veículo <b className="text-text-primary">{plate}</b> deixa de aparecer nas seleções de nova viagem e
      no rateio de custo fixo. Você pode reativá-lo depois.
    </>
  )
}
