import type { ReactNode } from 'react'
import { RoutePath } from '@/app/routePaths'
import type { MutationFormState } from '@/api/useMutationForm'
import { FormCard } from '@/design-system/components/FormCard'
import { DriverFormFields, type DriverFormMode } from './DriverFormFields'
import type { DriverFormValues } from './driverForm'

interface DriverFormCardProps {
  form: MutationFormState<DriverFormValues>
  mode: DriverFormMode
  savedMessage?: string | null
}

export function DriverFormCard({ form, mode, savedMessage }: DriverFormCardProps): ReactNode {
  return (
    <FormCard
      title="Dados do motorista"
      cancelTo={RoutePath.drivers}
      isPending={form.isPending}
      failureMessage={form.failureMessage}
      savedMessage={savedMessage}
      onSubmit={form.submit}
    >
      <DriverFormFields form={form} mode={mode} />
    </FormCard>
  )
}
