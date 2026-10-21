import type { ParsedForm } from '@/api/useMutationForm'
import { formatCnpj, formatPhone } from '@/design-system/documentFormat'
import { parseLocaleNumber } from '@/design-system/numberFormat'
import {
  isBillingCycle,
  type BillingCycle,
  type CreateCustomerRequest,
  type Customer,
  type UpdateCustomerRequest,
} from './customer'

/** Campos como o usuário digita; o prazo vira número só no envio. */
export interface CustomerFormValues {
  corporateName: string
  tradeName: string
  cnpj: string
  phone: string
  email: string
  billingCycle: BillingCycle
  paymentTermDays: string
}

export const EMPTY_CUSTOMER_FORM: CustomerFormValues = {
  corporateName: '',
  tradeName: '',
  cnpj: '',
  phone: '',
  email: '',
  billingCycle: 'Monthly',
  paymentTermDays: '',
}

const NOT_A_WHOLE_NUMBER_MESSAGE = 'Informe um número inteiro de dias.'

/**
 * Converte o cadastro; prazo em branco é "não informado" (null). CNPJ, telefone e e-mail
 * vão como digitados: o servidor normaliza e valida (TASK-006).
 * @example parseCreateCustomerForm({ ...EMPTY_CUSTOMER_FORM, corporateName: 'Agro Sul', cnpj: '11.222.333/0001-81' })
 */
export function parseCreateCustomerForm(values: CustomerFormValues): ParsedForm<CreateCustomerRequest> {
  const profile = parseUpdateCustomerForm(values)
  if (profile.kind === 'invalid') return profile

  return { kind: 'parsed', request: { ...profile.request, cnpj: values.cnpj } }
}

export function parseUpdateCustomerForm(values: CustomerFormValues): ParsedForm<UpdateCustomerRequest> {
  const paymentTermDays = parsePaymentTerm(values.paymentTermDays)
  if (paymentTermDays === undefined)
    return { kind: 'invalid', fieldErrors: { paymentTermDays: NOT_A_WHOLE_NUMBER_MESSAGE } }

  const { corporateName, tradeName, phone, email, billingCycle } = values
  return {
    kind: 'parsed',
    request: { corporateName, tradeName, phone, email, billingCycle, paymentTermDays },
  }
}

/** Valores de edição a partir do cliente salvo, com CNPJ e telefone mascarados. */
export function formValuesOf(customer: Customer): CustomerFormValues {
  return {
    corporateName: customer.corporateName ?? '',
    tradeName: customer.tradeName ?? '',
    cnpj: formatCnpj(customer.cnpj),
    phone: formatPhone(customer.phone),
    email: customer.email ?? '',
    billingCycle:
      customer.billingCycle && isBillingCycle(customer.billingCycle) ? customer.billingCycle : 'Monthly',
    paymentTermDays: customer.paymentTermDays?.toString() ?? '',
  }
}

// undefined = ilegível; null = não informado.
function parsePaymentTerm(input: string): number | null | undefined {
  if (input.trim() === '') return null
  const days = parseLocaleNumber(input)
  return days !== null && Number.isInteger(days) ? days : undefined
}
