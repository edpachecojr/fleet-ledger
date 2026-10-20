import { useEffect, type ReactNode } from 'react'
import { classNames } from '../classNames'

type DrawerSide = 'start' | 'end'

interface DrawerProps {
  open: boolean
  onClose: () => void
  side: DrawerSide
  label: string
  className?: string
  children: ReactNode
}

const SIDE_CLASSES: Record<DrawerSide, string> = {
  start: 'start-0',
  end: 'end-0 shadow-[var(--shadow-drawer)]',
}

/** Painel lateral sobre um scrim desfocado; fecha no Esc e no clique fora. */
export function Drawer({ open, onClose, side, label, className, children }: DrawerProps): ReactNode {
  useCloseOnEscape(open, onClose)
  if (!open) return null

  return (
    <div className="fixed inset-0 z-50">
      <button
        type="button"
        aria-label="Fechar"
        className="absolute inset-0 bg-overlay backdrop-blur-[8px]"
        onClick={onClose}
      />
      <div
        role="dialog"
        aria-modal="true"
        aria-label={label}
        className={classNames('absolute inset-y-0 flex max-w-full flex-col', SIDE_CLASSES[side], className)}
      >
        {children}
      </div>
    </div>
  )
}

function useCloseOnEscape(open: boolean, onClose: () => void): void {
  useEffect(() => {
    if (!open) return
    const closeOnEscape = (event: KeyboardEvent): void => {
      if (event.key === 'Escape') onClose()
    }
    window.addEventListener('keydown', closeOnEscape)
    return () => {
      window.removeEventListener('keydown', closeOnEscape)
    }
  }, [open, onClose])
}
