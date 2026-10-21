import type { ReactNode } from 'react'
import { Badge } from '@/design-system/components/Badge'
import { Card } from '@/design-system/components/Card'
import { SummaryList, SummaryRow } from '@/design-system/components/SummaryList'
import { formatQuantity } from '@/design-system/numberFormat'
import { formatProvisionRate, presentVehicleActivity, presentVehicleStatus, type Vehicle } from './vehicle'

/** Situação atual do veículo, com o status operacional em destaque (TASK-005). */
export function VehicleSummaryCard({ vehicle }: { vehicle: Vehicle }): ReactNode {
  const status = presentVehicleStatus(vehicle.status)
  const activity = presentVehicleActivity(vehicle)

  return (
    <Card title="Situação">
      <SummaryList>
        <SummaryRow term="Status operacional">
          <Badge tone={status.tone}>{status.label}</Badge>
        </SummaryRow>
        <SummaryRow term="Cadastro">
          <Badge tone={activity.tone}>{activity.label}</Badge>
        </SummaryRow>
        <SummaryRow term="Hodômetro atual">{formatQuantity(vehicle.currentOdometerKm ?? 0, 'km')}</SummaryRow>
        <SummaryRow term="Hodômetro inicial">
          {formatQuantity(vehicle.initialOdometerKm ?? 0, 'km')}
        </SummaryRow>
        <SummaryRow term="Taxa de provisão">
          {formatProvisionRate(vehicle.maintenanceProvisionRatePerKm)}
        </SummaryRow>
      </SummaryList>
    </Card>
  )
}
