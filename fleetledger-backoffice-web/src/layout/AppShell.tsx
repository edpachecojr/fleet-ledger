import { useState, type ReactNode } from 'react'
import { Outlet } from 'react-router'
import { Drawer } from '@/design-system/components/Drawer'
import { ThemeSettingsDrawer } from '@/design-system/settings/ThemeSettingsDrawer'
import { Header } from './Header'
import { Sidebar } from './Sidebar'

/** Layout de todas as páginas autenticadas: sidebar fixa ≥1024px, gaveta abaixo disso. */
export function AppShell(): ReactNode {
  const [navigationOpen, setNavigationOpen] = useState(false)
  const closeNavigation = (): void => {
    setNavigationOpen(false)
  }

  return (
    <div className="grid min-h-screen bg-bg lg:grid-cols-[var(--sidebar-width)_1fr]">
      <Sidebar className="sticky top-0 hidden h-screen lg:flex" />
      <Drawer open={navigationOpen} onClose={closeNavigation} side="start" label="Menu">
        <Sidebar className="flex h-full" onNavigate={closeNavigation} />
      </Drawer>
      <div className="flex min-w-0 flex-col">
        <Header
          onOpenNavigation={() => {
            setNavigationOpen(true)
          }}
        />
        <main className="grid content-start gap-6 p-4 sm:p-6">
          <Outlet />
        </main>
      </div>
      <ThemeSettingsDrawer />
    </div>
  )
}
