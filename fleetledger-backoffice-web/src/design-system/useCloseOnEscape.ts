import { useEffect } from 'react'

/** Fecha sobreposições (Drawer, ConfirmDialog) no Esc enquanto estão abertas. */
export function useCloseOnEscape(open: boolean, onClose: () => void): void {
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
