import { SearchX } from 'lucide-react'
import type { ReactNode } from 'react'
import { useParams } from 'react-router'
import { RoutePath } from '@/app/routePaths'
import { ButtonLink } from '@/design-system/components/ButtonLink'
import { Card } from '@/design-system/components/Card'
import { EmptyState } from '@/design-system/components/EmptyState'
import { PageHeader } from '@/design-system/components/PageHeader'
import { Skeleton } from '@/design-system/components/Skeleton'
import type { Customer } from './customer'
import { CustomerActivityAction } from './CustomerActivityAction'
import { CustomerEditForm } from './CustomerEditForm'
import { CustomerSummaryCard } from './CustomerSummaryCard'
import { useCustomer } from './useCustomers'

export function CustomerDetailPage(): ReactNode {
  const { customerId = '' } = useParams()
  const customer = useCustomer(customerId)

  if (customer.isPending) return <Skeleton className="h-64" />
  if (customer.isError)
    return (
      <CustomerMissing
        title="Cliente indisponível"
        description="Não foi possível carregar o cliente agora."
      />
    )
  if (!customer.data)
    return (
      <CustomerMissing title="Cliente não encontrado" description="O cliente não existe ou foi removido." />
    )

  return <CustomerDetail customer={customer.data} />
}

function CustomerDetail({ customer }: { customer: Customer }): ReactNode {
  return (
    <>
      <PageHeader
        title={customer.corporateName ?? ''}
        description={customer.tradeName ?? ''}
        actions={<CustomerActivityAction customer={customer} />}
      />
      <div className="grid items-start gap-6 lg:grid-cols-[1fr_2fr]">
        <CustomerSummaryCard customer={customer} />
        <CustomerEditForm customer={customer} />
      </div>
    </>
  )
}

function CustomerMissing({ title, description }: { title: string; description: string }): ReactNode {
  return (
    <Card>
      <EmptyState
        icon={SearchX}
        title={title}
        description={description}
        action={
          <ButtonLink to={RoutePath.customers} variant="outline">
            Voltar para clientes
          </ButtonLink>
        }
      />
    </Card>
  )
}
