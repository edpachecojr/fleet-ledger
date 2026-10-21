import type { ReactNode } from 'react'

/** Lista de termo/valor dentro de um Card, com régua tracejada entre as linhas (como o DataTable). */
export function SummaryList({ children }: { children: ReactNode }): ReactNode {
  return <dl className="grid">{children}</dl>
}

export function SummaryRow({ term, children }: { term: string; children: ReactNode }): ReactNode {
  return (
    <div className="flex items-center justify-between gap-3 border-b border-dashed border-divider py-3 last:border-b-0">
      <dt className="text-text-secondary">{term}</dt>
      <dd className="text-end font-semibold tabular-nums">{children}</dd>
    </div>
  )
}
