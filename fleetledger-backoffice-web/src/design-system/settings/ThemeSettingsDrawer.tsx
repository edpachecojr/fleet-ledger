import { SlidersHorizontal, X } from 'lucide-react'
import { useState, type ReactNode } from 'react'
import { Drawer } from '../components/Drawer'
import { IconButton } from '../components/IconButton'
import { ThemeSettingsForm } from './ThemeSettingsForm'

/** Botão flutuante que abre o painel de modo, cor, densidade e direção. */
export function ThemeSettingsDrawer(): ReactNode {
  const [open, setOpen] = useState(false)
  const close = (): void => {
    setOpen(false)
  }

  return (
    <>
      <SettingsFab
        onClick={() => {
          setOpen(true)
        }}
      />
      <Drawer
        open={open}
        onClose={close}
        side="end"
        label="Configurações"
        className="w-[var(--drawer-width)] bg-surface"
      >
        <header className="flex h-14 items-center justify-between border-b border-divider ps-5 pe-3 text-base font-semibold">
          Configurações
          <IconButton label="Fechar configurações" onClick={close}>
            <X />
          </IconButton>
        </header>
        <ThemeSettingsForm />
      </Drawer>
    </>
  )
}

function SettingsFab({ onClick }: { onClick: () => void }): ReactNode {
  return (
    <button
      type="button"
      aria-label="Configurações de aparência"
      onClick={onClick}
      className="fixed end-6 bottom-6 z-40 grid size-11 place-items-center rounded-full bg-primary text-on-primary shadow-[var(--shadow-primary)]"
    >
      <SlidersHorizontal className="size-4" aria-hidden />
    </button>
  )
}
