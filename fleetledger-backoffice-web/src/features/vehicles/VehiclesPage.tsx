import { Plus } from 'lucide-react'
import type { ReactNode } from 'react'
import { RoutePath } from '@/app/routePaths'
import { ButtonLink } from '@/design-system/components/ButtonLink'
import { PageHeader } from '@/design-system/components/PageHeader'
import { VehicleListCard } from './VehicleListCard'

export function VehiclesPage(): ReactNode {
  return (
    <>
      <PageHeader
        title="Veículos"
        description="Frota cadastrada, com status operacional e hodômetro atual."
        actions={
          <ButtonLink to={RoutePath.newVehicle} icon={<Plus />}>
            Novo veículo
          </ButtonLink>
        }
      />
      <VehicleListCard />
    </>
  )
}
