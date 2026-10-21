import { Plus } from 'lucide-react'
import type { ReactNode } from 'react'
import { RoutePath } from '@/app/routePaths'
import { ButtonLink } from '@/design-system/components/ButtonLink'
import { PageHeader } from '@/design-system/components/PageHeader'
import { CustomerListCard } from './CustomerListCard'

export function CustomersPage(): ReactNode {
  return (
    <>
      <PageHeader
        title="Clientes"
        description="Base de clientes com ciclo de faturamento e prazo de pagamento."
        actions={
          <ButtonLink to={RoutePath.newCustomer} icon={<Plus />}>
            Novo cliente
          </ButtonLink>
        }
      />
      <CustomerListCard />
    </>
  )
}
