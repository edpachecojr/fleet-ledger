import type { components } from '@/api/generated/schema'
import type { SelectOption } from '@/design-system/components/SelectField'

export type Customer = components['schemas']['Customer']
export type BillingCycle = components['schemas']['EBillingCycle']
export type CustomerPage = components['schemas']['PagedResponseOfListOfCustomer']
export type CreateCustomerRequest = components['schemas']['CreateCustomerRequest']
export type UpdateCustomerRequest = components['schemas']['UpdateCustomerRequest']

const BILLING_CYCLE_LABELS: Record<BillingCycle, string> = {
  Weekly: 'Semanal',
  Biweekly: 'Quinzenal',
  Monthly: 'Mensal',
}

export const BILLING_CYCLE_OPTIONS: readonly SelectOption[] = [
  { value: 'Weekly', label: BILLING_CYCLE_LABELS.Weekly },
  { value: 'Biweekly', label: BILLING_CYCLE_LABELS.Biweekly },
  { value: 'Monthly', label: BILLING_CYCLE_LABELS.Monthly },
]

/**
 * Ciclo de faturamento em pt-BR.
 * @example labelBillingCycle('Biweekly') // 'Quinzenal'
 */
export function labelBillingCycle(cycle: BillingCycle | undefined): string {
  return cycle ? BILLING_CYCLE_LABELS[cycle] : ''
}

export function isBillingCycle(value: string): value is BillingCycle {
  return value in BILLING_CYCLE_LABELS
}

/**
 * Prazo de pagamento; sem prazo informado mostra travessão.
 * @example formatPaymentTerm(30) // '30 dias'
 */
export function formatPaymentTerm(days: number | null | undefined): string {
  if (days === null || days === undefined) return '—'
  return days === 1 ? '1 dia' : `${days} dias`
}
