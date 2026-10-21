import { Save } from 'lucide-react'
import type { ReactNode, SubmitEvent } from 'react'
import { Button } from './Button'
import { ButtonLink } from './ButtonLink'
import { Card } from './Card'
import { InlineMessage } from './InlineMessage'

interface FormCardProps {
  title: string
  cancelTo: string
  isPending: boolean
  failureMessage: string | null
  savedMessage?: string | null | undefined
  onSubmit: (event: SubmitEvent<HTMLFormElement>) => void
  children: ReactNode
}

/**
 * Formulário num Card (card FormControls): campos, retorno do envio e as ações no fim, à
 * direita: "Cancelar" (outline, volta para `cancelTo`) e "Salvar" (primária).
 * @example <FormCard title="Dados do veículo" cancelTo="/veiculos" onSubmit={form.submit} ...>campos</FormCard>
 */
export function FormCard(props: FormCardProps): ReactNode {
  const { title, cancelTo, isPending, failureMessage, savedMessage, onSubmit, children } = props

  return (
    <Card title={title}>
      <form className="grid gap-6" onSubmit={onSubmit} noValidate>
        {children}
        {failureMessage && <InlineMessage tone="error">{failureMessage}</InlineMessage>}
        {!failureMessage && savedMessage && <InlineMessage tone="success">{savedMessage}</InlineMessage>}
        <div className="flex justify-end gap-3">
          <ButtonLink to={cancelTo} variant="outline">
            Cancelar
          </ButtonLink>
          <Button type="submit" icon={<Save />} disabled={isPending}>
            {isPending ? 'Salvando…' : 'Salvar'}
          </Button>
        </div>
      </form>
    </Card>
  )
}
