import type { ReactNode } from 'react'
import { NavLink } from 'react-router'
import { classNames } from '@/design-system/classNames'
import type { NavigationItem } from './navigation'

interface SidebarNavItemProps {
  item: NavigationItem
  onNavigate?: (() => void) | undefined
}

export function SidebarNavItem({
  item: { label, path, icon: Icon },
  onNavigate,
}: SidebarNavItemProps): ReactNode {
  return (
    <NavLink
      to={path}
      end
      onClick={onNavigate}
      className={({ isActive }) =>
        classNames(
          'flex h-[calc(36px*var(--density))] items-center gap-3 rounded-lg px-3 text-sm font-medium transition-colors',
          'hover:bg-sidebar-hover-bg hover:text-sidebar-text-active [&_svg]:size-[18px]',
          isActive && 'bg-sidebar-active-bg font-semibold text-sidebar-text-active [&_svg]:text-primary',
        )
      }
    >
      <Icon aria-hidden />
      {label}
    </NavLink>
  )
}
