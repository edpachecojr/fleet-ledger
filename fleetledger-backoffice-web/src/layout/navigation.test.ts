import { LayoutGrid } from 'lucide-react'
import { findNavigationItem, type NavigationSection } from './navigation'

const SECTIONS: NavigationSection[] = [
  { label: 'Cadastros', items: [{ label: 'Veículos', path: '/vehicles', icon: LayoutGrid }] },
]

describe('findNavigationItem', () => {
  it('finds the item whose path matches the current route', () => {
    expect(findNavigationItem('/vehicles', SECTIONS)?.label).toBe('Veículos')
  })

  it('returns undefined for routes outside the menu', () => {
    expect(findNavigationItem('/unknown', SECTIONS)).toBeUndefined()
  })
})
