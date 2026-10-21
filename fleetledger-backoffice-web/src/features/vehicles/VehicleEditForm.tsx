import { useState, type ReactNode } from 'react'
import { useMutationForm } from '@/api/useMutationForm'
import { useUpdateVehicle } from './useVehicleMutations'
import type { Vehicle } from './vehicle'
import { formValuesOf, parseUpdateVehicleForm } from './vehicleForm'
import { VehicleFormCard } from './VehicleFormCard'

const SAVED_MESSAGE = 'Veículo atualizado.'

/** Edição de modelo, capacidade e taxa; placa e hodômetro aparecem, mas não mudam aqui. */
export function VehicleEditForm({ vehicle }: { vehicle: Vehicle }): ReactNode {
  const [saved, setSaved] = useState(false)
  const form = useMutationForm({
    initialValues: formValuesOf(vehicle),
    parse: (values) => {
      setSaved(false)
      return parseUpdateVehicleForm(values)
    },
    mutation: useUpdateVehicle(vehicle.id ?? ''),
    onSaved: () => {
      setSaved(true)
    },
  })

  return (
    <VehicleFormCard
      title="Dados do veículo"
      form={form}
      mode="edit"
      savedMessage={saved ? SAVED_MESSAGE : null}
    />
  )
}
