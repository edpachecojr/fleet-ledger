import type { ReactNode } from 'react'
import { useNavigate } from 'react-router'
import { vehicleDetailPath } from '@/app/routePaths'
import { useMutationForm } from '@/api/useMutationForm'
import { PageHeader } from '@/design-system/components/PageHeader'
import { useCreateVehicle } from './useVehicleMutations'
import { EMPTY_VEHICLE_FORM, parseCreateVehicleForm } from './vehicleForm'
import { VehicleFormCard } from './VehicleFormCard'

/** Cadastro: o veículo nasce Disponível e, salvo, abre no detalhe. */
export function NewVehiclePage(): ReactNode {
  const navigate = useNavigate()
  const form = useMutationForm({
    initialValues: EMPTY_VEHICLE_FORM,
    parse: parseCreateVehicleForm,
    mutation: useCreateVehicle(),
    onSaved: (vehicle) => {
      void navigate(vehicleDetailPath(vehicle.id ?? ''))
    },
  })

  return (
    <>
      <PageHeader
        title="Novo veículo"
        description="O veículo começa com status Disponível e hodômetro atual igual ao inicial."
      />
      <VehicleFormCard title="Dados do veículo" form={form} mode="create" />
    </>
  )
}
