import { Power, PowerOff } from 'lucide-react'
import { useState, type ReactNode } from 'react'
import { Button } from './Button'
import { ConfirmDialog } from './ConfirmDialog'
import { InlineMessage } from './InlineMessage'

export interface ActivityToggleProps {
  isActive: boolean
  /** Título da confirmação, nomeando o objeto: "Inativar veículo?". */
  confirmTitle: string
  /** Consequência da inativação, nomeando o registro (card ConfirmDialog). */
  consequence: ReactNode
  pending: boolean
  rejection: string | null
  onChange: (deactivate: boolean, done: () => void) => void
}

/**
 * Inativar pede confirmação (o registro sai das seleções); reativar é direto, porque não
 * tira nada de lugar nenhum. Usado por veículos, clientes e motoristas.
 * @example <ActivityToggle isActive confirmTitle="Inativar veículo?" consequence="..." onChange={...} />
 */
export function ActivityToggle(props: ActivityToggleProps): ReactNode {
  const { isActive, pending, rejection, onChange } = props
  if (!isActive)
    return (
      <Button
        variant="outline"
        icon={<Power />}
        disabled={pending}
        onClick={() => {
          onChange(false, () => undefined)
        }}
      >
        Reativar
      </Button>
    )

  return (
    <>
      <DeactivateWithConfirmation {...props} />
      {rejection && <InlineMessage tone="error">{rejection}</InlineMessage>}
    </>
  )
}

function DeactivateWithConfirmation({
  confirmTitle,
  consequence,
  pending,
  onChange,
}: ActivityToggleProps): ReactNode {
  const [confirming, setConfirming] = useState(false)
  const open = (): void => {
    setConfirming(true)
  }
  const close = (): void => {
    setConfirming(false)
  }

  return (
    <>
      <Button variant="outline" icon={<PowerOff />} onClick={open}>
        Inativar
      </Button>
      <ConfirmDialog
        {...{ open: confirming, title: confirmTitle, message: consequence, pending, onCancel: close }}
        confirmLabel="Inativar"
        onConfirm={() => {
          onChange(true, close)
        }}
      />
    </>
  )
}
