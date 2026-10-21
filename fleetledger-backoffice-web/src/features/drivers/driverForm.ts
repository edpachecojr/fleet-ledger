import type { FieldErrors } from '@/api/mutationOutcome'
import type { ParsedForm } from '@/api/useMutationForm'
import { formatCpf, formatPhone } from '@/design-system/documentFormat'
import { parseLocaleNumber } from '@/design-system/numberFormat'
import {
  isCnhCategory,
  type CnhCategory,
  type CreateDriverRequest,
  type Driver,
  type UpdateDriverRequest,
} from './driver'

/** Campos como o usuário digita; datas vêm do input de data ("AAAA-MM-DD" ou vazio). */
export interface DriverFormValues {
  name: string
  cpf: string
  phone: string
  cnhNumber: string
  cnhCategory: CnhCategory
  cnhExpiresOn: string
  toxicologyExamExpiresOn: string
  defaultCommissionPercent: string
  bankDetails: string
}

export const EMPTY_DRIVER_FORM: DriverFormValues = {
  name: '',
  cpf: '',
  phone: '',
  cnhNumber: '',
  cnhCategory: 'E',
  cnhExpiresOn: '',
  toxicologyExamExpiresOn: '',
  defaultCommissionPercent: '0,00',
  bankDetails: '',
}

/**
 * Converte o cadastro; validade da CNH e comissão ilegíveis viram erro do campo antes da
 * API (as regras de verdade — CPF, faixas, duplicidade — continuam no servidor).
 * @example parseCreateDriverForm({ ...EMPTY_DRIVER_FORM, cpf: '529.982.247-25', cnhExpiresOn: '2028-05-10' })
 */
export function parseCreateDriverForm(values: DriverFormValues): ParsedForm<CreateDriverRequest> {
  const profile = parseUpdateDriverForm(values)
  if (profile.kind === 'invalid') return profile

  return { kind: 'parsed', request: { ...profile.request, cpf: values.cpf } }
}

export function parseUpdateDriverForm(values: DriverFormValues): ParsedForm<UpdateDriverRequest> {
  const commission = parseLocaleNumber(values.defaultCommissionPercent)
  const fieldErrors: FieldErrors = {
    ...(values.cnhExpiresOn ? {} : { cnhExpiresOn: 'Informe a validade da CNH.' }),
    ...(commission === null ? { defaultCommissionPercent: 'Informe um número.' } : {}),
  }
  if (commission === null || Object.keys(fieldErrors).length > 0) return { kind: 'invalid', fieldErrors }

  const { name, phone, cnhNumber, cnhCategory, cnhExpiresOn, bankDetails } = values
  const toxicologyExamExpiresOn = values.toxicologyExamExpiresOn || null
  return {
    kind: 'parsed',
    request: {
      name,
      phone,
      cnhNumber,
      cnhCategory,
      cnhExpiresOn,
      toxicologyExamExpiresOn,
      defaultCommissionPercent: commission,
      bankDetails,
    },
  }
}

/** Valores de edição a partir do motorista salvo, com CPF e telefone mascarados. */
export function formValuesOf(driver: Driver): DriverFormValues {
  return {
    name: driver.name ?? '',
    cpf: formatCpf(driver.cpf),
    phone: formatPhone(driver.phone),
    cnhNumber: driver.cnhNumber ?? '',
    cnhCategory: driver.cnhCategory && isCnhCategory(driver.cnhCategory) ? driver.cnhCategory : 'E',
    cnhExpiresOn: driver.cnhExpiresOn ?? '',
    toxicologyExamExpiresOn: driver.toxicologyExamExpiresOn ?? '',
    defaultCommissionPercent: (driver.defaultCommissionPercent ?? 0).toFixed(2).replace('.', ','),
    bankDetails: driver.bankDetails ?? '',
  }
}
