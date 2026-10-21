import type { ParsedForm } from '@/api/useMutationForm'
import { parseLocaleNumber } from '@/design-system/numberFormat'
import type { CreateVehicleRequest, UpdateVehicleRequest, Vehicle } from './vehicle'

/** Campos como o usuário digita (texto); viram números só no envio. */
export interface VehicleFormValues {
  plate: string
  model: string
  capacityKg: string
  initialOdometerKm: string
  maintenanceProvisionRatePerKm: string
}

export const EMPTY_VEHICLE_FORM: VehicleFormValues = {
  plate: '',
  model: '',
  capacityKg: '',
  initialOdometerKm: '0',
  maintenanceProvisionRatePerKm: '0,00',
}

const NOT_A_NUMBER_MESSAGE = 'Informe um número.'
const NUMERIC_FIELDS = ['capacityKg', 'initialOdometerKm', 'maintenanceProvisionRatePerKm'] as const

type NumericField = (typeof NUMERIC_FIELDS)[number]

/**
 * Converte o formulário de cadastro; número ilegível vira erro do campo antes de ir à API
 * (as regras de verdade — faixas, placa, duplicidade — continuam no servidor).
 * @example parseCreateVehicleForm({ ...EMPTY_VEHICLE_FORM, capacityKg: '27.000' })
 */
export function parseCreateVehicleForm(values: VehicleFormValues): ParsedForm<CreateVehicleRequest> {
  const numbers = parseNumbers(values, NUMERIC_FIELDS)
  if (numbers.kind === 'invalid') return numbers

  return { kind: 'parsed', request: { plate: values.plate, model: values.model, ...numbers.request } }
}

export function parseUpdateVehicleForm(values: VehicleFormValues): ParsedForm<UpdateVehicleRequest> {
  const numbers = parseNumbers(values, ['capacityKg', 'maintenanceProvisionRatePerKm'])
  if (numbers.kind === 'invalid') return numbers

  return { kind: 'parsed', request: { model: values.model, ...numbers.request } }
}

/** Valores de edição a partir do veículo salvo, com a taxa em vírgula decimal. */
export function formValuesOf(vehicle: Vehicle): VehicleFormValues {
  return {
    plate: vehicle.plate ?? '',
    model: vehicle.model ?? '',
    capacityKg: String(vehicle.capacityKg ?? ''),
    initialOdometerKm: String(vehicle.initialOdometerKm ?? 0),
    maintenanceProvisionRatePerKm: (vehicle.maintenanceProvisionRatePerKm ?? 0).toFixed(2).replace('.', ','),
  }
}

function parseNumbers(
  values: VehicleFormValues,
  fields: readonly NumericField[],
): ParsedForm<Partial<Record<NumericField, number>>> {
  const numbers: Partial<Record<NumericField, number>> = {}
  const fieldErrors: Partial<Record<NumericField, string>> = {}
  for (const field of fields) {
    const value = parseLocaleNumber(values[field])
    if (value === null) fieldErrors[field] = NOT_A_NUMBER_MESSAGE
    else numbers[field] = value
  }

  return Object.keys(fieldErrors).length > 0
    ? { kind: 'invalid', fieldErrors }
    : { kind: 'parsed', request: numbers }
}
