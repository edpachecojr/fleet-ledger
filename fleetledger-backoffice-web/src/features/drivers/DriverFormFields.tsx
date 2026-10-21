import type { ReactNode } from 'react'
import { fieldBinding, type MutationFormState } from '@/api/useMutationForm'
import { SelectField } from '@/design-system/components/SelectField'
import { TextAreaField } from '@/design-system/components/TextAreaField'
import { TextField } from '@/design-system/components/TextField'
import { CNH_CATEGORY_OPTIONS, isCnhCategory } from './driver'
import type { DriverFormValues } from './driverForm'

export type DriverFormMode = 'create' | 'edit'

interface DriverFormFieldsProps {
  form: MutationFormState<DriverFormValues>
  mode: DriverFormMode
}

type DriverForm = MutationFormState<DriverFormValues>

const CPF_HELP = 'Com ou sem pontuação: 529.982.247-25.'
const LOCKED_CPF_HELP = 'O CPF identifica o motorista e não pode ser alterado.'

/** Campos do motorista em 2 colunas (card FormControls). Na edição, o CPF fica bloqueado. */
export function DriverFormFields({ form, mode }: DriverFormFieldsProps): ReactNode {
  const editing = mode === 'edit'

  return (
    <div className="grid gap-x-6 gap-y-4 md:grid-cols-2">
      <TextField label="Nome" {...fieldBinding(form, 'name')} />
      <TextField
        label="CPF"
        disabled={editing}
        help={editing ? LOCKED_CPF_HELP : CPF_HELP}
        {...fieldBinding(form, 'cpf')}
      />
      <TextField label="Telefone" type="tel" help="Com DDD." {...fieldBinding(form, 'phone')} />
      <TextField
        label="Comissão padrão (%)"
        inputMode="decimal"
        help="Alterar não afeta viagens já abertas."
        {...fieldBinding(form, 'defaultCommissionPercent')}
      />
      <CnhFields form={form} />
      <TextAreaField
        label="Dados bancários ou PIX"
        help="Opcional; usado para pagar o acerto."
        className="md:col-span-2"
        {...fieldBinding(form, 'bankDetails')}
      />
    </div>
  )
}

function CnhFields({ form }: { form: DriverForm }): ReactNode {
  return (
    <>
      <TextField
        label="Número da CNH"
        inputMode="numeric"
        help="11 dígitos."
        {...fieldBinding(form, 'cnhNumber')}
      />
      <SelectField
        label="Categoria da CNH"
        options={CNH_CATEGORY_OPTIONS}
        value={form.values.cnhCategory}
        error={form.fieldErrors.cnhCategory}
        onChange={(event) => {
          if (isCnhCategory(event.target.value)) form.setField('cnhCategory', event.target.value)
        }}
      />
      <TextField label="Validade da CNH" type="date" {...fieldBinding(form, 'cnhExpiresOn')} />
      <TextField
        label="Validade do exame toxicológico"
        type="date"
        help="Opcional; em branco não conta na situação documental."
        {...fieldBinding(form, 'toxicologyExamExpiresOn')}
      />
    </>
  )
}
