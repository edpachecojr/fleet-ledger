import { isCnhCategory, isDocumentStatusFilter, presentDocumentStatus } from './driver'

describe('presentDocumentStatus', () => {
  it.each([
    ['Regular', 'Regular', 'success'],
    ['ExpiringSoon', 'Vencendo', 'warning'],
    ['Expired', 'Vencido', 'error'],
  ] as const)('maps %s to "%s" (%s)', (status, label, tone) => {
    expect(presentDocumentStatus(status)).toEqual({ label, tone })
  })
})

describe('isCnhCategory', () => {
  it('accepts the CNH categories only', () => {
    expect(isCnhCategory('AE')).toBe(true)
    expect(isCnhCategory('F')).toBe(false)
  })
})

describe('isDocumentStatusFilter', () => {
  it('accepts every status and "all"', () => {
    expect(['all', 'Regular', 'ExpiringSoon', 'Expired'].every(isDocumentStatusFilter)).toBe(true)
    expect(isDocumentStatusFilter('Unknown')).toBe(false)
  })
})
