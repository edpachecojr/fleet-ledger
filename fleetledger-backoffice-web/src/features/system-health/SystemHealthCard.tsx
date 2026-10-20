import { RefreshCw } from 'lucide-react'
import type { ReactNode } from 'react'
import { Card } from '@/design-system/components/Card'
import { IconButton } from '@/design-system/components/IconButton'
import { Skeleton } from '@/design-system/components/Skeleton'
import {
  labelHealthCheck,
  presentHealthStatus,
  UNREACHABLE_PRESENTATION,
  type SystemHealth,
} from './systemHealth'
import { SystemHealthRow } from './SystemHealthRow'
import { useSystemHealth } from './useSystemHealth'

const API_ROW_LABEL = 'API'

/** Estado da API e de cada dependência, lido de GET /health a cada 30 s. */
export function SystemHealthCard(): ReactNode {
  const { data, isPending, isError, refetch, isFetching } = useSystemHealth()
  const refreshButton = (
    <IconButton label="Verificar agora" disabled={isFetching} onClick={() => void refetch()}>
      <RefreshCw />
    </IconButton>
  )

  return (
    <Card title="Status do sistema" actions={refreshButton}>
      {isPending && <Skeleton className="h-24" />}
      {isError && (
        <ul>
          <SystemHealthRow label={API_ROW_LABEL} status={UNREACHABLE_PRESENTATION} />
        </ul>
      )}
      {data && <HealthReport health={data} />}
    </Card>
  )
}

function HealthReport({ health }: { health: SystemHealth }): ReactNode {
  return (
    <ul>
      <SystemHealthRow label={API_ROW_LABEL} status={presentHealthStatus(health.status)} />
      {health.checks.map((check) => (
        <SystemHealthRow
          key={check.name}
          label={labelHealthCheck(check.name)}
          status={presentHealthStatus(check.status)}
          durationMs={check.durationMs}
        />
      ))}
    </ul>
  )
}
