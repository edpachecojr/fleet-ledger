import { isActivityFilter, listQueryParams, presentActivity } from './listQuery'

describe('listQueryParams', () => {
  it('sends only active records by default and trims the search', () => {
    expect(listQueryParams({ search: '  abc ', activity: 'active', page: 1 })).toEqual({
      search: 'abc',
      isActive: true,
      pageNumber: 1,
      pageSize: 25,
    })
  })

  it('omits isActive and an empty search when listing everything', () => {
    expect(listQueryParams({ search: ' ', activity: 'all', page: 3 })).toEqual({
      pageNumber: 3,
      pageSize: 25,
    })
  })

  it('asks for inactive records', () => {
    expect(listQueryParams({ search: '', activity: 'inactive', page: 1 }).isActive).toBe(false)
  })
})

describe('isActivityFilter', () => {
  it('accepts the known filters only', () => {
    expect(isActivityFilter('inactive')).toBe(true)
    expect(isActivityFilter('deleted')).toBe(false)
  })
})

describe('presentActivity', () => {
  it('tells active and inactive records apart', () => {
    expect(presentActivity(true)).toEqual({ label: 'Ativo', tone: 'success' })
    expect(presentActivity(false)).toEqual({ label: 'Inativo', tone: 'neutral' })
  })
})
