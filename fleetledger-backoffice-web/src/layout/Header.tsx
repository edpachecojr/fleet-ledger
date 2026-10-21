import { PanelLeft } from 'lucide-react'
import type { ReactNode } from 'react'
import { useLocation } from 'react-router'
import { PRODUCT_NAME } from '@/app/product'
import { IconButton } from '@/design-system/components/IconButton'
import { LogoutButton } from './LogoutButton'
import { findNavigationItem } from './navigation'
import { ThemeModeToggle } from './ThemeModeToggle'

interface HeaderProps {
  onOpenNavigation: () => void
}

/** Barra superior fixa e translúcida: menu (telas pequenas), título da página e ferramentas. */
export function Header({ onOpenNavigation }: HeaderProps): ReactNode {
  const { pathname } = useLocation()
  const title = findNavigationItem(pathname)?.label ?? PRODUCT_NAME

  return (
    <header className="sticky top-0 z-30 flex h-[var(--header-height)] items-center gap-2 border-b border-divider bg-header-bg px-4 backdrop-blur-[8px] sm:px-6">
      <IconButton label="Abrir menu" className="lg:hidden" onClick={onOpenNavigation}>
        <PanelLeft />
      </IconButton>
      <span className="text-sm font-medium">{title}</span>
      <span className="flex-1" />
      <ThemeModeToggle />
      <LogoutButton />
    </header>
  )
}
