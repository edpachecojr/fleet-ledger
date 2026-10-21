import type { ReactNode } from 'react'
import { RoutePath } from '@/app/routePaths'
import type { MutationFormState } from '@/api/useMutationForm'
import { FormCard } from '@/design-system/components/FormCard'
import { CustomerFormFields, type CustomerFormMode } from './CustomerFormFields'
import type { CustomerFormValues } from './customerForm'

interface CustomerFormCardProps {
  form: MutationFormState<CustomerFormValues>
  mode: CustomerFormMode
  savedMessage?: string | null
}

export function CustomerFormCard({ form, mode, savedMessage }: CustomerFormCardProps): ReactNode {
  return (
    <FormCard
      title="Dados do cliente"
      cancelTo={RoutePath.customers}
      isPending={form.isPending}
      failureMessage={form.failureMessage}
      savedMessage={savedMessage}
      onSubmit={form.submit}
    >
      <CustomerFormFields form={form} mode={mode} />
    </FormCard>
  )
}
