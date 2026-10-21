import { Save } from 'lucide-react'
import type { ReactNode } from 'react'
import { RoutePath } from '@/app/routePaths'
import type { MutationFormState } from '@/api/useMutationForm'
import { Button } from '@/design-system/components/Button'
import { ButtonLink } from '@/design-system/components/ButtonLink'
import { Card } from '@/design-system/components/Card'
import { InlineMessage } from '@/design-system/components/InlineMessage'
import { VehicleFormFields, type VehicleFormMode } from './VehicleFormFields'
import type { VehicleFormValues } from './vehicleForm'

interface VehicleFormCardProps {
  title: string
  form: MutationFormState<VehicleFormValues>
  mode: VehicleFormMode
  savedMessage?: string | null
}

/** Formulário num Card; ações no fim, à direita: "Cancelar" (outline) e "Salvar" (primária). */
export function VehicleFormCard({ title, form, mode, savedMessage }: VehicleFormCardProps): ReactNode {
  return (
    <Card title={title}>
      <form className="grid gap-6" onSubmit={form.submit} noValidate>
        <VehicleFormFields form={form} mode={mode} />
        {form.failureMessage && <InlineMessage tone="error">{form.failureMessage}</InlineMessage>}
        {!form.failureMessage && savedMessage && <InlineMessage tone="success">{savedMessage}</InlineMessage>}
        <div className="flex justify-end gap-3">
          <ButtonLink to={RoutePath.vehicles} variant="outline">
            Cancelar
          </ButtonLink>
          <Button type="submit" icon={<Save />} disabled={form.isPending}>
            {form.isPending ? 'Salvando…' : 'Salvar'}
          </Button>
        </div>
      </form>
    </Card>
  )
}
