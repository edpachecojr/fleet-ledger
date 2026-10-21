import { SearchX } from 'lucide-react'
import type { ReactNode } from 'react'
import { useParams } from 'react-router'
import { RoutePath } from '@/app/routePaths'
import { ButtonLink } from '@/design-system/components/ButtonLink'
import { Card } from '@/design-system/components/Card'
import { EmptyState } from '@/design-system/components/EmptyState'
import { PageHeader } from '@/design-system/components/PageHeader'
import { Skeleton } from '@/design-system/components/Skeleton'
import type { Driver } from './driver'
import { DriverActivityAction } from './DriverActivityAction'
import { DriverEditForm } from './DriverEditForm'
import { DriverSummaryCard } from './DriverSummaryCard'
import { useDriver } from './useDrivers'

export function DriverDetailPage(): ReactNode {
  const { driverId = '' } = useParams()
  const driver = useDriver(driverId)

  if (driver.isPending) return <Skeleton className="h-64" />
  if (driver.isError)
    return (
      <DriverMissing
        title="Motorista indisponível"
        description="Não foi possível carregar o motorista agora."
      />
    )
  if (!driver.data)
    return (
      <DriverMissing title="Motorista não encontrado" description="O motorista não existe ou foi removido." />
    )

  return <DriverDetail driver={driver.data} />
}

function DriverDetail({ driver }: { driver: Driver }): ReactNode {
  return (
    <>
      <PageHeader title={driver.name ?? ''} actions={<DriverActivityAction driver={driver} />} />
      <div className="grid items-start gap-6 lg:grid-cols-[1fr_2fr]">
        <DriverSummaryCard driver={driver} />
        <DriverEditForm driver={driver} />
      </div>
    </>
  )
}

function DriverMissing({ title, description }: { title: string; description: string }): ReactNode {
  return (
    <Card>
      <EmptyState
        icon={SearchX}
        title={title}
        description={description}
        action={
          <ButtonLink to={RoutePath.drivers} variant="outline">
            Voltar para motoristas
          </ButtonLink>
        }
      />
    </Card>
  )
}
