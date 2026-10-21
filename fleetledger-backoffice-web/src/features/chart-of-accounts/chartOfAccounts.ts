import type { components } from '@/api/generated/schema'

export type LedgerAccountGroup = components['schemas']['LedgerAccountGroup']
export type LedgerAccount = components['schemas']['LedgerAccount']
export type LedgerEntryContext = NonNullable<components['schemas']['ELedgerEntryContext']>

const ENTRY_CONTEXT_LABELS: Record<LedgerEntryContext, string> = {
  Trip: 'Viagem',
  FixedExpense: 'Despesa fixa',
  Maintenance: 'Manutenção',
  Billing: 'Faturamento',
}

/**
 * Onde a conta aceita lançamento manual; `null` quando só o sistema lança nela.
 * @example labelEntryContext('Trip') // 'Viagem'
 */
export function labelEntryContext(entryContext: LedgerAccount['entryContext']): string | null {
  return entryContext ? ENTRY_CONTEXT_LABELS[entryContext] : null
}

/** "3 · Despesas variáveis de viagem": código e nome do grupo como título do card. */
export function titleOfGroup(group: LedgerAccountGroup): string {
  return `${group.code ?? ''} · ${group.name ?? ''}`
}
