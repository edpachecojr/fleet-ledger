import type { ReactNode } from 'react'
import { Badge } from '@/design-system/components/Badge'
import type { HealthStatusPresentation } from './systemHealth'

interface SystemHealthRowProps {
  label: string
  status: HealthStatusPresentation
  durationMs?: number
}

export function SystemHealthRow({ label, status, durationMs }: SystemHealthRowProps): ReactNode {
  return (
    <li className="flex items-center justify-between gap-3 border-b border-dashed border-divider py-3 last:border-b-0">
      <span className="font-medium">{label}</span>
      <span className="flex items-center gap-3">
        {durationMs !== undefined && (
          <span className="text-xs text-text-secondary tabular-nums">{Math.round(durationMs)} ms</span>
        )}
        <Badge tone={status.tone}>{status.label}</Badge>
      </span>
    </li>
  )
}
