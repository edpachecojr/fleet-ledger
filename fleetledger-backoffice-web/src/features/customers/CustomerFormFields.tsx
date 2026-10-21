import type { ReactNode } from 'react'
import { fieldBinding, type MutationFormState } from '@/api/useMutationForm'
import { SelectField } from '@/design-system/components/SelectField'
import { TextField } from '@/design-system/components/TextField'
import { BILLING_CYCLE_OPTIONS, isBillingCycle } from './customer'
import type { CustomerFormValues } from './customerForm'

export type CustomerFormMode = 'create' | 'edit'

interface CustomerFormFieldsProps {
  form: MutationFormState<CustomerFormValues>
  mode: CustomerFormMode
}

const CNPJ_HELP = 'Com ou sem pontuação: 11.222.333/0001-81.'
const LOCKED_CNPJ_HELP = 'O CNPJ identifica o cliente e não pode ser alterado.'

/** Campos do cliente em 2 colunas (card FormControls). Na edição, o CNPJ fica bloqueado. */
export function CustomerFormFields({ form, mode }: CustomerFormFieldsProps): ReactNode {
  const editing = mode === 'edit'

  return (
    <div className="grid gap-x-6 gap-y-4 md:grid-cols-2">
      <TextField label="Razão social" {...fieldBinding(form, 'corporateName')} />
      <TextField label="Nome fantasia" help="Opcional." {...fieldBinding(form, 'tradeName')} />
      <TextField
        label="CNPJ"
        disabled={editing}
        help={editing ? LOCKED_CNPJ_HELP : CNPJ_HELP}
        {...fieldBinding(form, 'cnpj')}
      />
      <BillingCycleField form={form} />
      <TextField label="Telefone" type="tel" help="Opcional, com DDD." {...fieldBinding(form, 'phone')} />
      <TextField label="E-mail" type="email" help="Opcional." {...fieldBinding(form, 'email')} />
      <TextField
        label="Prazo de pagamento (dias)"
        inputMode="numeric"
        help="Opcional; sugere o vencimento da fatura."
        {...fieldBinding(form, 'paymentTermDays')}
      />
    </div>
  )
}

function BillingCycleField({ form }: { form: MutationFormState<CustomerFormValues> }): ReactNode {
  return (
    <SelectField
      label="Ciclo de faturamento"
      options={BILLING_CYCLE_OPTIONS}
      value={form.values.billingCycle}
      error={form.fieldErrors.billingCycle}
      onChange={(event) => {
        if (isBillingCycle(event.target.value)) form.setField('billingCycle', event.target.value)
      }}
    />
  )
}
