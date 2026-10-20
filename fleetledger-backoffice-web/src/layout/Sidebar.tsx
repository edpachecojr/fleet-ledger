import type { ReactNode } from 'react'
import { PRODUCT_NAME } from '@/app/product'
import { classNames } from '@/design-system/classNames'
import { NAVIGATION_SECTIONS } from './navigation'
import { SidebarNavItem } from './SidebarNavItem'

interface SidebarProps {
  className?: string
  onNavigate?: () => void
}

/** Menu lateral grafite (igual nos dois temas), com marca e seções de navegação. */
export function Sidebar({ className, onNavigate }: SidebarProps): ReactNode {
  return (
    <aside
      className={classNames(
        'w-[var(--sidebar-width)] flex-col border-e border-sidebar-border bg-sidebar-bg text-sidebar-text',
        className,
      )}
    >
      <div className="flex h-[var(--header-height)] items-center gap-2.5 border-b border-sidebar-border px-4 text-lg font-bold text-sidebar-text-active">
        <span className="grid size-8 place-items-center rounded-md bg-primary text-on-primary">F</span>
        {PRODUCT_NAME}
      </div>
      <nav aria-label="Menu principal" className="grid content-start gap-1 overflow-y-auto p-3">
        {NAVIGATION_SECTIONS.map((section) => (
          <div key={section.label} className="grid gap-1">
            <p className="px-2 pt-4 pb-2 text-xs font-semibold tracking-[0.06em] text-sidebar-label uppercase">
              {section.label}
            </p>
            {section.items.map((item) => (
              <SidebarNavItem key={item.path} item={item} onNavigate={onNavigate} />
            ))}
          </div>
        ))}
      </nav>
    </aside>
  )
}
