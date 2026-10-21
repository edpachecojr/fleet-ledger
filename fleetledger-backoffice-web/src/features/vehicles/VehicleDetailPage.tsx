import { SearchX } from 'lucide-react'
import type { ReactNode } from 'react'
import { useParams } from 'react-router'
import { RoutePath } from '@/app/routePaths'
import { ButtonLink } from '@/design-system/components/ButtonLink'
import { Card } from '@/design-system/components/Card'
import { EmptyState } from '@/design-system/components/EmptyState'
import { PageHeader } from '@/design-system/components/PageHeader'
import { Skeleton } from '@/design-system/components/Skeleton'
import { useVehicle } from './useVehicles'
import type { Vehicle } from './vehicle'
import { VehicleActivityAction } from './VehicleActivityAction'
import { VehicleEditForm } from './VehicleEditForm'
import { VehicleStatusAction } from './VehicleStatusAction'
import { VehicleSummaryCard } from './VehicleSummaryCard'

export function VehicleDetailPage(): ReactNode {
  const { vehicleId = '' } = useParams()
  const vehicle = useVehicle(vehicleId)

  if (vehicle.isPending) return <Skeleton className="h-64" />
  if (vehicle.isError)
    return (
      <VehicleMissing title="Veículo indisponível" description="Não foi possível carregar o veículo agora." />
    )
  if (!vehicle.data)
    return (
      <VehicleMissing title="Veículo não encontrado" description="O veículo não existe ou foi removido." />
    )

  return <VehicleDetail vehicle={vehicle.data} />
}

function VehicleDetail({ vehicle }: { vehicle: Vehicle }): ReactNode {
  return (
    <>
      <PageHeader
        title={vehicle.plate ?? ''}
        description={vehicle.model ?? ''}
        actions={<VehicleHeaderActions vehicle={vehicle} />}
      />
      <div className="grid items-start gap-6 lg:grid-cols-[1fr_2fr]">
        <VehicleSummaryCard vehicle={vehicle} />
        <VehicleEditForm vehicle={vehicle} />
      </div>
    </>
  )
}

function VehicleHeaderActions({ vehicle }: { vehicle: Vehicle }): ReactNode {
  return (
    <>
      <VehicleStatusAction vehicle={vehicle} />
      <VehicleActivityAction vehicle={vehicle} />
    </>
  )
}

function VehicleMissing({ title, description }: { title: string; description: string }): ReactNode {
  return (
    <Card>
      <EmptyState
        icon={SearchX}
        title={title}
        description={description}
        action={
          <ButtonLink to={RoutePath.vehicles} variant="outline">
            Voltar para veículos
          </ButtonLink>
        }
      />
    </Card>
  )
}
