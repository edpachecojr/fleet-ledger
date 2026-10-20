import type { HealthStatus, SystemHealth } from '@/features/system-health/systemHealth'

export function systemHealthReport(databaseStatus: HealthStatus): SystemHealth {
  return {
    status: databaseStatus,
    totalDurationMs: 12.4,
    checks: [{ name: 'database', status: databaseStatus, durationMs: 10.6, description: null }],
  }
}
