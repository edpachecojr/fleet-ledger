import { ListTree } from 'lucide-react'
import type { ReactNode } from 'react'
import { Card } from '@/design-system/components/Card'
import { EmptyState } from '@/design-system/components/EmptyState'
import { PageHeader } from '@/design-system/components/PageHeader'
import { Skeleton } from '@/design-system/components/Skeleton'
import { LedgerAccountGroupCard } from './LedgerAccountGroupCard'
import { useChartOfAccounts } from './useChartOfAccounts'

/** Consulta do Plano de Contas, somente leitura (TASK-003): não há ação de alterar contas. */
export function ChartOfAccountsPage(): ReactNode {
  return (
    <>
      <PageHeader
        title="Plano de contas"
        description="Contas fixas que classificam os lançamentos e montam a DRE. Não podem ser alteradas."
      />
      <ChartOfAccountsContent />
    </>
  )
}

function ChartOfAccountsContent(): ReactNode {
  const { data, isPending, isError } = useChartOfAccounts()
  if (isPending) return <Skeleton className="h-64" />
  if (isError) return <ChartUnavailable />

  return data.map((group) => <LedgerAccountGroupCard key={group.id} group={group} />)
}

function ChartUnavailable(): ReactNode {
  return (
    <Card>
      <EmptyState
        icon={ListTree}
        title="Plano de contas indisponível"
        description="Não foi possível carregar as contas agora. Tente novamente em instantes."
      />
    </Card>
  )
}
