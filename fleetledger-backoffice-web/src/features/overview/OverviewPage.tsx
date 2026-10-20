import { Truck } from 'lucide-react'
import type { ReactNode } from 'react'
import { Card } from '@/design-system/components/Card'
import { EmptyState } from '@/design-system/components/EmptyState'
import { PageHeader } from '@/design-system/components/PageHeader'
import { SystemHealthCard } from '@/features/system-health/SystemHealthCard'

export function OverviewPage(): ReactNode {
  return (
    <>
      <PageHeader title="Visão geral" description="Acompanhe a operação e o resultado da sua frota." />
      <div className="grid gap-6 lg:grid-cols-[2fr_1fr]">
        <Card>
          <EmptyState
            icon={Truck}
            title="Indicadores em breve"
            description="Os indicadores de viagens, frota e resultado aparecem aqui conforme os módulos forem entregues."
          />
        </Card>
        <SystemHealthCard />
      </div>
    </>
  )
}
