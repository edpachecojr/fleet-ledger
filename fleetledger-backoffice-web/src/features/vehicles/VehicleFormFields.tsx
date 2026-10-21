import type { ReactNode } from 'react'
import type { MutationFormState } from '@/api/useMutationForm'
import { TextField } from '@/design-system/components/TextField'
import type { VehicleFormValues } from './vehicleForm'

export type VehicleFormMode = 'create' | 'edit'

interface VehicleFormFieldsProps {
  form: MutationFormState<VehicleFormValues>
  mode: VehicleFormMode
}

const PLATE_HELP = 'Formato antigo (ABC-1234) ou Mercosul (ABC1D23).'
const LOCKED_PLATE_HELP = 'A placa identifica o veículo e não pode ser alterada.'

/**
 * Campos do veículo em 2 colunas (card FormControls). Na edição, a placa fica bloqueada e
 * o hodômetro some: ele só muda pelo sistema (TASK-004).
 */
export function VehicleFormFields({ form, mode }: VehicleFormFieldsProps): ReactNode {
  const editing = mode === 'edit'

  return (
    <div className="grid gap-x-6 gap-y-4 md:grid-cols-2">
      <TextField
        label="Placa"
        disabled={editing}
        help={editing ? LOCKED_PLATE_HELP : PLATE_HELP}
        {...bindField(form, 'plate')}
      />
      <TextField label="Modelo" {...bindField(form, 'model')} />
      <TextField label="Capacidade (kg)" inputMode="numeric" {...bindField(form, 'capacityKg')} />
      {!editing && (
        <TextField
          label="Hodômetro inicial (km)"
          inputMode="numeric"
          {...bindField(form, 'initialOdometerKm')}
        />
      )}
      <TextField
        label="Taxa de provisão de manutenção (R$/km)"
        inputMode="decimal"
        help="Alterar vale para as viagens fechadas depois da alteração."
        {...bindField(form, 'maintenanceProvisionRatePerKm')}
      />
    </div>
  )
}

function bindField(
  form: MutationFormState<VehicleFormValues>,
  field: keyof VehicleFormValues,
): { value: string; error: string | undefined; onChange: (event: { target: { value: string } }) => void } {
  return {
    value: form.values[field],
    error: form.fieldErrors[field],
    onChange: (event) => {
      form.setField(field, event.target.value)
    },
  }
}
