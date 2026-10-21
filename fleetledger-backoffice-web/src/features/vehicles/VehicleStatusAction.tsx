import { CircleCheck, Wrench } from 'lucide-react'
import type { ReactNode } from 'react'
import { Button } from '@/design-system/components/Button'
import { InlineMessage } from '@/design-system/components/InlineMessage'
import type { ManualVehicleStatus } from './changeVehicleStatus'
import { useChangeVehicleStatus } from './useVehicleMutations'
import type { Vehicle } from './vehicle'

interface StatusActionCopy {
  target: ManualVehicleStatus
  label: string
  icon: ReactNode
}

const TO_MAINTENANCE: StatusActionCopy = {
  target: 'InMaintenance',
  label: 'Colocar em manutenção',
  icon: <Wrench />,
}
const TO_AVAILABLE: StatusActionCopy = {
  target: 'Available',
  label: 'Liberar para uso',
  icon: <CircleCheck />,
}

/**
 * Ação manual de status: Disponível ↔ Em manutenção, só em veículo ativo. Em viagem não
 * tem ação aqui: quem tira o veículo da viagem é o fechamento ou o cancelamento dela.
 */
export function manualStatusActionOf(vehicle: Vehicle): StatusActionCopy | null {
  if (vehicle.isActive === false) return null
  if (vehicle.status === 'Available') return TO_MAINTENANCE
  return vehicle.status === 'InMaintenance' ? TO_AVAILABLE : null
}

export function VehicleStatusAction({ vehicle }: { vehicle: Vehicle }): ReactNode {
  const statusChange = useChangeVehicleStatus(vehicle.id ?? '')
  const action = manualStatusActionOf(vehicle)
  if (!action) return null

  return (
    <>
      <Button
        variant="outline"
        icon={action.icon}
        disabled={statusChange.isPending}
        onClick={() => {
          statusChange.mutate(action.target)
        }}
      >
        {action.label}
      </Button>
      {statusChange.data?.kind === 'rejected' && (
        <InlineMessage tone="error">{statusChange.data.message}</InlineMessage>
      )}
    </>
  )
}
