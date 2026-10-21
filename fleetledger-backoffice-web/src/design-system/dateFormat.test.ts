import { formatDateOnly } from './dateFormat'

describe('formatDateOnly', () => {
  it('formats an ISO date as day/month/year', () => {
    expect(formatDateOnly('2026-11-10')).toBe('10/11/2026')
  })

  it('shows a dash when the date is missing or malformed', () => {
    expect(formatDateOnly(null)).toBe('—')
    expect(formatDateOnly('10/11/2026')).toBe('—')
  })
})
