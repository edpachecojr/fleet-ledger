import { useId, type ReactNode } from 'react'
import { useCloseOnEscape } from '../useCloseOnEscape'
import { Button } from './Button'

interface ConfirmDialogProps {
  open: boolean
  title: string
  message: ReactNode
  confirmLabel: string
  pending?: boolean
  onCancel: () => void
  onConfirm: () => void
}

/**
 * Confirmação de ação destrutiva (card ConfirmDialog): scrim `overlay` com blur, painel de
 * até 420px; "Cancelar" (outline, foco inicial) e o verbo em Danger. Esc cancela.
 * @example <ConfirmDialog open title="Inativar veículo?" message="..." confirmLabel="Inativar" ... />
 */
export function ConfirmDialog(props: ConfirmDialogProps): ReactNode {
  const { open, title, message, confirmLabel, pending = false, onCancel, onConfirm } = props
  const titleId = useId()
  useCloseOnEscape(open, onCancel)
  if (!open) return null

  return (
    <div className="fixed inset-0 z-50 grid place-items-center bg-overlay p-4 backdrop-blur-[8px]">
      <div
        role="alertdialog"
        aria-modal="true"
        aria-labelledby={titleId}
        className="grid w-full max-w-[420px] gap-4 rounded-2xl bg-surface p-6 shadow-[var(--shadow-dialog)]"
      >
        <h2 id={titleId} className="text-lg font-semibold">
          {title}
        </h2>
        <p className="text-text-secondary">{message}</p>
        <div className="flex justify-end gap-3">
          <Button variant="outline" autoFocus onClick={onCancel}>
            Cancelar
          </Button>
          <Button variant="danger" disabled={pending} onClick={onConfirm}>
            {confirmLabel}
          </Button>
        </div>
      </div>
    </div>
  )
}
