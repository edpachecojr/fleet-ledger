import type { ReactNode } from 'react'
import { Badge } from '@/design-system/components/Badge'
import { Card } from '@/design-system/components/Card'
import { formatQuantity } from '@/design-system/numberFormat'
import { formatProvisionRate, presentVehicleActivity, presentVehicleStatus, type Vehicle } from './vehicle'

/** Situação atual do veículo, com o status operacional em destaque (TASK-005). */
export function VehicleSummaryCard({ vehicle }: { vehicle: Vehicle }): ReactNode {
  const status = presentVehicleStatus(vehicle.status)
  const activity = presentVehicleActivity(vehicle)

  return (
    <Card title="Situação">
      <dl className="grid">
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
      </dl>
    </Card>
  )
}

function SummaryRow({ term, children }: { term: string; children: ReactNode }): ReactNode {
  return (
    <div className="flex items-center justify-between gap-3 border-b border-dashed border-divider py-3 last:border-b-0">
      <dt className="text-text-secondary">{term}</dt>
      <dd className="font-semibold tabular-nums">{children}</dd>
    </div>
  )
}
