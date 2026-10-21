import type { components } from '@/api/generated/schema'
import { presentActivity, type ListQuery } from '@/api/listQuery'
import type { BadgePresentation } from '@/design-system/components/Badge'
import type { SelectOption } from '@/design-system/components/SelectField'
import { formatCurrency } from '@/design-system/numberFormat'

export type Vehicle = components['schemas']['Vehicle']
export type VehicleStatus = components['schemas']['EVehicleStatus']
export type VehiclePage = components['schemas']['PagedResponseOfListOfVehicle']
export type CreateVehicleRequest = components['schemas']['CreateVehicleRequest']
export type UpdateVehicleRequest = components['schemas']['UpdateVehicleRequest']

const STATUS_PRESENTATION: Record<VehicleStatus, BadgePresentation> = {
  Available: { label: 'Disponível', tone: 'success' },
  OnTrip: { label: 'Em viagem', tone: 'info' },
  InMaintenance: { label: 'Em manutenção', tone: 'warning' },
}

export type VehicleStatusFilter = VehicleStatus | 'all'

/** Listagem de veículos: busca, situação e página comuns, mais o status operacional. */
export interface VehicleListQuery extends ListQuery {
  status: VehicleStatusFilter
}

export const VEHICLE_STATUS_FILTER_OPTIONS: readonly SelectOption[] = [
  { value: 'all', label: 'Todos os status' },
  { value: 'Available', label: STATUS_PRESENTATION.Available.label },
  { value: 'OnTrip', label: STATUS_PRESENTATION.OnTrip.label },
  { value: 'InMaintenance', label: STATUS_PRESENTATION.InMaintenance.label },
]

export function isVehicleStatusFilter(value: string): value is VehicleStatusFilter {
  return VEHICLE_STATUS_FILTER_OPTIONS.some((option) => option.value === value)
}

/**
 * Badge do status operacional (card Badge: verde disponível, azul em viagem, âmbar em manutenção).
 * @example presentVehicleStatus('InMaintenance') // { label: 'Em manutenção', tone: 'warning' }
 */
export function presentVehicleStatus(status: VehicleStatus | undefined): BadgePresentation {
  return STATUS_PRESENTATION[status ?? 'Available']
}

export function presentVehicleActivity(vehicle: Vehicle): BadgePresentation {
  return presentActivity(vehicle.isActive)
}

/**
 * Taxa de provisão de manutenção por km.
 * @example formatProvisionRate(0.15) // 'R$ 0,15/km'
 */
export function formatProvisionRate(ratePerKm: number | undefined): string {
  return `${formatCurrency(ratePerKm ?? 0)}/km`
}
