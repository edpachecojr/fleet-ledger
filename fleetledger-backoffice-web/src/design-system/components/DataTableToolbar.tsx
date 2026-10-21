import type { ReactNode } from 'react'

/** Faixa acima da tabela (card DataTable): busca, filtros e ações alinhadas ao fim. */
export function DataTableToolbar({ children }: { children: ReactNode }): ReactNode {
  return <div className="flex flex-wrap items-end gap-3 p-[calc(24px*var(--density))] pb-4">{children}</div>
}
