import type { ReactNode } from 'react'
import { Link } from 'react-router'
import { vehicleDetailPath } from '@/app/routePaths'
import { Badge } from '@/design-system/components/Badge'
import type { DataTableColumn } from '@/design-system/components/DataTable'
import { formatQuantity } from '@/design-system/numberFormat'
import { presentVehicleActivity, presentVehicleStatus, type Vehicle } from './vehicle'

export const VEHICLE_TABLE_COLUMNS: readonly DataTableColumn<Vehicle>[] = [
  { key: 'plate', header: 'Placa', render: (vehicle) => <PlateLink vehicle={vehicle} /> },
  { key: 'model', header: 'Modelo', render: (vehicle) => vehicle.model },
  {
    key: 'capacity',
    header: 'Capacidade',
    align: 'end',
    render: (vehicle) => formatQuantity(vehicle.capacityKg ?? 0, 'kg'),
  },
  {
    key: 'odometer',
    header: 'Hodômetro atual',
    align: 'end',
    render: (vehicle) => formatQuantity(vehicle.currentOdometerKm ?? 0, 'km'),
  },
  {
    key: 'status',
    header: 'Status',
    render: (vehicle) => <PresentedBadge {...presentVehicleStatus(vehicle.status)} />,
  },
  {
    key: 'activity',
    header: 'Situação',
    render: (vehicle) => <PresentedBadge {...presentVehicleActivity(vehicle)} />,
  },
]

function PlateLink({ vehicle }: { vehicle: Vehicle }): ReactNode {
  return (
    <Link
      to={vehicleDetailPath(vehicle.id ?? '')}
      className="font-mono font-semibold text-primary hover:underline"
    >
      {vehicle.plate}
    </Link>
  )
}

function PresentedBadge({ label, tone }: ReturnType<typeof presentVehicleStatus>): ReactNode {
  return <Badge tone={tone}>{label}</Badge>
}
