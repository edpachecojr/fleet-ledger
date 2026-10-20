import { systemHealthReport } from '@/test/fakes/systemHealthReports'
import { isSystemHealth, labelHealthCheck, presentHealthStatus } from './systemHealth'

describe('presentHealthStatus', () => {
  it.each([
    ['Healthy', 'Operacional', 'success'],
    ['Degraded', 'Degradado', 'warning'],
    ['Unhealthy', 'Indisponível', 'error'],
  ] as const)('maps %s to "%s" with %s tone', (status, label, tone) => {
    expect(presentHealthStatus(status)).toEqual({ label, tone })
  })
})

describe('labelHealthCheck', () => {
  it('translates known checks and keeps unknown names', () => {
    expect(labelHealthCheck('database')).toBe('Banco de dados')
    expect(labelHealthCheck('cache')).toBe('cache')
  })
})

describe('isSystemHealth', () => {
  it('accepts a health report and rejects other payloads', () => {
    expect(isSystemHealth(systemHealthReport('Healthy'))).toBe(true)
    expect(isSystemHealth('Bad Gateway')).toBe(false)
    expect(isSystemHealth(null)).toBe(false)
  })
})
