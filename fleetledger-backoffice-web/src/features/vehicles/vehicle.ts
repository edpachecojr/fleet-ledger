import type { components } from '@/api/generated/schema'
import type { BadgeTone } from '@/design-system/components/Badge'
import { formatCurrency } from '@/design-system/numberFormat'

export type Vehicle = components['schemas']['Vehicle']
export type VehicleStatus = components['schemas']['EVehicleStatus']
export type VehiclePage = components['schemas']['PagedResponseOfListOfVehicle']
export type CreateVehicleRequest = components['schemas']['CreateVehicleRequest']
export type UpdateVehicleRequest = components['schemas']['UpdateVehicleRequest']

export interface BadgePresentation {
  label: string
  tone: BadgeTone
}

const STATUS_PRESENTATION: Record<VehicleStatus, BadgePresentation> = {
  Available: { label: 'Disponível', tone: 'success' },
  OnTrip: { label: 'Em viagem', tone: 'info' },
  InMaintenance: { label: 'Em manutenção', tone: 'warning' },
}

const ACTIVE: BadgePresentation = { label: 'Ativo', tone: 'success' }
const INACTIVE: BadgePresentation = { label: 'Inativo', tone: 'neutral' }

/**
 * Badge do status operacional (card Badge: verde disponível, azul em viagem, âmbar em manutenção).
 * @example presentVehicleStatus('InMaintenance') // { label: 'Em manutenção', tone: 'warning' }
 */
export function presentVehicleStatus(status: VehicleStatus | undefined): BadgePresentation {
  return STATUS_PRESENTATION[status ?? 'Available']
}

export function presentVehicleActivity(vehicle: Vehicle): BadgePresentation {
  return vehicle.isActive === false ? INACTIVE : ACTIVE
}

/**
 * Taxa de provisão de manutenção por km.
 * @example formatProvisionRate(0.15) // 'R$ 0,15/km'
 */
export function formatProvisionRate(ratePerKm: number | undefined): string {
  return `${formatCurrency(ratePerKm ?? 0)}/km`
}
