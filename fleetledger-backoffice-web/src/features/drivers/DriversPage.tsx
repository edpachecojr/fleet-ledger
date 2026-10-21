import { Plus } from 'lucide-react'
import type { ReactNode } from 'react'
import { RoutePath } from '@/app/routePaths'
import { ButtonLink } from '@/design-system/components/ButtonLink'
import { PageHeader } from '@/design-system/components/PageHeader'
import { DriverListCard } from './DriverListCard'

export function DriversPage(): ReactNode {
  return (
    <>
      <PageHeader
        title="Motoristas"
        description="Motoristas com comissão padrão e vencimento da CNH e do exame toxicológico."
        actions={
          <ButtonLink to={RoutePath.newDriver} icon={<Plus />}>
            Novo motorista
          </ButtonLink>
        }
      />
      <DriverListCard />
    </>
  )
}
