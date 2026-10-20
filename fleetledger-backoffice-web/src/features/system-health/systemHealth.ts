import type { BadgeTone } from '@/design-system/components/Badge'
import type { components } from '@/api/generated/schema'

export type SystemHealth = components['schemas']['HealthCheckResponse']
export type HealthStatus = components['schemas']['HealthStatus']

export interface HealthStatusPresentation {
  label: string
  tone: BadgeTone
}

const STATUS_PRESENTATION: Record<HealthStatus, HealthStatusPresentation> = {
  Healthy: { label: 'Operacional', tone: 'success' },
  Degraded: { label: 'Degradado', tone: 'warning' },
  Unhealthy: { label: 'Indisponível', tone: 'error' },
}

// Nomes das verificações registradas na API (HealthCheckExtension).
const CHECK_LABELS: Record<string, string> = {
  database: 'Banco de dados',
}

// A API pode não responder (proxy sem upstream, rede): não há relatório para exibir.
export const UNREACHABLE_PRESENTATION: HealthStatusPresentation = { label: 'Inacessível', tone: 'error' }

export function presentHealthStatus(status: HealthStatus): HealthStatusPresentation {
  return STATUS_PRESENTATION[status]
}

export function labelHealthCheck(checkName: string): string {
  return CHECK_LABELS[checkName] ?? checkName
}

/** Distingue o corpo de /health de outros erros (ex.: proxy sem API respondendo em texto). */
export function isSystemHealth(value: unknown): value is SystemHealth {
  return typeof value === 'object' && value !== null && 'status' in value && 'checks' in value
}
