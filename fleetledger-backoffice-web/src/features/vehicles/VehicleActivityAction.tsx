import { Power, PowerOff } from 'lucide-react'
import { useState, type ReactNode } from 'react'
import { Button } from '@/design-system/components/Button'
import { ConfirmDialog } from '@/design-system/components/ConfirmDialog'
import { InlineMessage } from '@/design-system/components/InlineMessage'
import { useChangeVehicleActivity } from './useVehicleMutations'
import type { Vehicle } from './vehicle'

/**
 * Inativar pede confirmação (o veículo sai das seleções e do rateio); reativar é direto,
 * porque não tira nada de lugar nenhum.
 */
export function VehicleActivityAction({ vehicle }: { vehicle: Vehicle }): ReactNode {
  return vehicle.isActive === false ? (
    <ReactivateButton vehicle={vehicle} />
  ) : (
    <DeactivateAction vehicle={vehicle} />
  )
}

function ReactivateButton({ vehicle }: { vehicle: Vehicle }): ReactNode {
  const activity = useChangeVehicleActivity(vehicle.id ?? '')

  return (
    <Button
      variant="outline"
      icon={<Power />}
      disabled={activity.isPending}
      onClick={() => {
        activity.mutate('reactivate')
      }}
    >
      Reativar
    </Button>
  )
}

function DeactivateAction({ vehicle }: { vehicle: Vehicle }): ReactNode {
  const [confirming, setConfirming] = useState(false)
  const activity = useChangeVehicleActivity(vehicle.id ?? '')

  return (
    <>
      <Button
        variant="outline"
        icon={<PowerOff />}
        onClick={() => {
          setConfirming(true)
        }}
      >
        Inativar
      </Button>
      {activity.data?.kind === 'rejected' && (
        <InlineMessage tone="error">{activity.data.message}</InlineMessage>
      )}
      <DeactivationDialog
        plate={vehicle.plate ?? ''}
        open={confirming}
        activity={activity}
        onClose={() => {
          setConfirming(false)
        }}
      />
    </>
  )
}

interface DeactivationDialogProps {
  plate: string
  open: boolean
  activity: ReturnType<typeof useChangeVehicleActivity>
  onClose: () => void
}

function DeactivationDialog({ plate, open, activity, onClose }: DeactivationDialogProps): ReactNode {
  return (
    <ConfirmDialog
      open={open}
      title="Inativar veículo?"
      message={<DeactivationWarning plate={plate} />}
      confirmLabel="Inativar"
      pending={activity.isPending}
      onCancel={onClose}
      onConfirm={() => {
        activity.mutate('deactivate', { onSettled: onClose })
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
