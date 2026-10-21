import type { BadgePresentation } from '@/design-system/components/Badge'
import type { SelectOption } from '@/design-system/components/SelectField'

// Mesmo valor de PagingDefaults.PageSize na API.
export const LIST_PAGE_SIZE = 25

export type ActivityFilter = 'active' | 'inactive' | 'all'

export const ACTIVITY_FILTER_OPTIONS: readonly SelectOption[] = [
  { value: 'active', label: 'Ativos' },
  { value: 'inactive', label: 'Inativos' },
  { value: 'all', label: 'Todos' },
]

/** Busca, situação e página de uma listagem de cadastro (veículos, clientes, motoristas). */
export interface ListQuery {
  search: string
  activity: ActivityFilter
  page: number
}

export const INITIAL_LIST_QUERY: ListQuery = { search: '', activity: 'active', page: 1 }

/**
 * Parâmetros de query comuns às listagens de cadastro; `isActive` some quando são todos.
 * @example listQueryParams({ search: 'abc', activity: 'all', page: 2 }) // { search: 'abc', pageNumber: 2, pageSize: 25 }
 */
export function listQueryParams(query: ListQuery): {
  search?: string
  isActive?: boolean
  pageNumber: number
  pageSize: number
} {
  const search = query.search.trim()
  return {
    ...(search ? { search } : {}),
    ...(query.activity === 'all' ? {} : { isActive: query.activity === 'active' }),
    pageNumber: query.page,
    pageSize: LIST_PAGE_SIZE,
  }
}

export function isActivityFilter(value: string): value is ActivityFilter {
  return ACTIVITY_FILTER_OPTIONS.some((option) => option.value === value)
}

const ACTIVE: BadgePresentation = { label: 'Ativo', tone: 'success' }
const INACTIVE: BadgePresentation = { label: 'Inativo', tone: 'neutral' }

/**
 * Situação do cadastro (card Badge: Ativo em verde, Inativo neutro).
 * @example presentActivity(false) // { label: 'Inativo', tone: 'neutral' }
 */
export function presentActivity(isActive: boolean | undefined): BadgePresentation {
  return isActive === false ? INACTIVE : ACTIVE
}
