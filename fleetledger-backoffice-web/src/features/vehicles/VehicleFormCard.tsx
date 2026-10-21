import type { ReactNode } from 'react'
import { RoutePath } from '@/app/routePaths'
import type { MutationFormState } from '@/api/useMutationForm'
import { FormCard } from '@/design-system/components/FormCard'
import { VehicleFormFields, type VehicleFormMode } from './VehicleFormFields'
import type { VehicleFormValues } from './vehicleForm'

interface VehicleFormCardProps {
  title: string
  form: MutationFormState<VehicleFormValues>
  mode: VehicleFormMode
  savedMessage?: string | null
}

export function VehicleFormCard({ title, form, mode, savedMessage }: VehicleFormCardProps): ReactNode {
  return (
    <FormCard
      title={title}
      cancelTo={RoutePath.vehicles}
      isPending={form.isPending}
      failureMessage={form.failureMessage}
      savedMessage={savedMessage}
      onSubmit={form.submit}
    >
      <VehicleFormFields form={form} mode={mode} />
    </FormCard>
  )
}
