import type { components } from '@/api/generated/schema'
import type { ListQuery } from '@/api/listQuery'
import type { BadgePresentation } from '@/design-system/components/Badge'
import type { SelectOption } from '@/design-system/components/SelectField'

export type Driver = components['schemas']['Driver']
export type CnhCategory = components['schemas']['ECnhCategory']
export type DocumentStatus = components['schemas']['EDocumentStatus']
export type DriverPage = components['schemas']['PagedResponseOfListOfDriver']
export type CreateDriverRequest = components['schemas']['CreateDriverRequest']
export type UpdateDriverRequest = components['schemas']['UpdateDriverRequest']

const CNH_CATEGORIES: readonly CnhCategory[] = ['A', 'B', 'C', 'D', 'E', 'AB', 'AC', 'AD', 'AE']

export const CNH_CATEGORY_OPTIONS: readonly SelectOption[] = CNH_CATEGORIES.map((category) => ({
  value: category,
  label: category,
}))

export function isCnhCategory(value: string): value is CnhCategory {
  return CNH_CATEGORIES.some((category) => category === value)
}

// Vencido em vermelho, vencendo em âmbar, regular em verde (card Badge); o texto sempre vai junto.
const DOCUMENT_STATUS_PRESENTATION: Record<DocumentStatus, BadgePresentation> = {
  Regular: { label: 'Regular', tone: 'success' },
  ExpiringSoon: { label: 'Vencendo', tone: 'warning' },
  Expired: { label: 'Vencido', tone: 'error' },
}

/**
 * Situação documental calculada pela API para a data de hoje.
 * @example presentDocumentStatus('ExpiringSoon') // { label: 'Vencendo', tone: 'warning' }
 */
export function presentDocumentStatus(status: DocumentStatus | undefined): BadgePresentation {
  return DOCUMENT_STATUS_PRESENTATION[status ?? 'Regular']
}

export type DocumentStatusFilter = DocumentStatus | 'all'

/** Listagem de motoristas: busca, situação e página comuns, mais a situação documental. */
export interface DriverListQuery extends ListQuery {
  documentStatus: DocumentStatusFilter
}

export const DOCUMENT_STATUS_FILTER_OPTIONS: readonly SelectOption[] = [
  { value: 'all', label: 'Todos os documentos' },
  { value: 'Expired', label: DOCUMENT_STATUS_PRESENTATION.Expired.label },
  { value: 'ExpiringSoon', label: DOCUMENT_STATUS_PRESENTATION.ExpiringSoon.label },
  { value: 'Regular', label: DOCUMENT_STATUS_PRESENTATION.Regular.label },
]

export function isDocumentStatusFilter(value: string): value is DocumentStatusFilter {
  return DOCUMENT_STATUS_FILTER_OPTIONS.some((option) => option.value === value)
}
