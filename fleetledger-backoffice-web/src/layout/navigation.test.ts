import { LayoutGrid } from 'lucide-react'
import { findNavigationItem, isWithinItem, type NavigationSection } from './navigation'

const SECTIONS: NavigationSection[] = [
  { label: 'Cadastros', items: [{ label: 'Veículos', path: '/vehicles', icon: LayoutGrid }] },
]

describe('findNavigationItem', () => {
  it('finds the item whose path matches the current route', () => {
    expect(findNavigationItem('/vehicles', SECTIONS)?.label).toBe('Veículos')
  })

  it('finds the item of a sub-route', () => {
    expect(findNavigationItem('/vehicles/123', SECTIONS)?.label).toBe('Veículos')
  })

  it('returns undefined for routes outside the menu', () => {
    expect(findNavigationItem('/unknown', SECTIONS)).toBeUndefined()
  })
})

describe('isWithinItem', () => {
  it('matches the root only with itself', () => {
    expect(isWithinItem('/', '/')).toBe(true)
    expect(isWithinItem('/veiculos', '/')).toBe(false)
  })

  it('does not match a path that merely shares the prefix', () => {
    expect(isWithinItem('/veiculos-antigos', '/veiculos')).toBe(false)
  })
})
