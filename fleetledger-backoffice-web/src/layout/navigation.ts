import { LayoutGrid, ListTree, type LucideIcon } from 'lucide-react'
import { RoutePath } from '@/app/routePaths'

export interface NavigationItem {
  label: string
  path: string
  icon: LucideIcon
}

export interface NavigationSection {
  label: string
  items: readonly NavigationItem[]
}

// Cada módulo entregue acrescenta seu item aqui (ver docs/ROADMAP.md).
export const NAVIGATION_SECTIONS: readonly NavigationSection[] = [
  { label: 'Dashboards', items: [{ label: 'Visão geral', path: RoutePath.overview, icon: LayoutGrid }] },
  {
    label: 'Financeiro',
    items: [{ label: 'Plano de contas', path: RoutePath.chartOfAccounts, icon: ListTree }],
  },
]

/**
 * Item de menu correspondente à rota atual, usado como título da página no header.
 * @example findNavigationItem('/') // { label: 'Visão geral', ... }
 */
export function findNavigationItem(
  pathname: string,
  sections: readonly NavigationSection[] = NAVIGATION_SECTIONS,
): NavigationItem | undefined {
  return sections.flatMap((section) => section.items).find((item) => item.path === pathname)
}
