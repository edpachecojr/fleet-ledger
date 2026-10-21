import type { ReactNode } from 'react'
import { Badge } from '@/design-system/components/Badge'
import { Card } from '@/design-system/components/Card'
import { DataTable, type DataTableColumn } from '@/design-system/components/DataTable'
import {
  labelEntryContext,
  titleOfGroup,
  type LedgerAccount,
  type LedgerAccountGroup,
} from './chartOfAccounts'

const COLUMNS: readonly DataTableColumn<LedgerAccount>[] = [
  {
    key: 'code',
    header: 'Código',
    widthClass: 'w-28',
    render: (account) => <span className="font-mono text-[13px]">{account.code}</span>,
  },
  { key: 'name', header: 'Conta', render: (account) => <span className="font-medium">{account.name}</span> },
  {
    key: 'entryContext',
    header: 'Lançamento manual',
    widthClass: 'w-56',
    render: (account) => <EntryContextCell account={account} />,
  },
]

/** Um grupo da DRE e suas contas analíticas. */
export function LedgerAccountGroupCard({ group }: { group: LedgerAccountGroup }): ReactNode {
  const title = titleOfGroup(group)

  return (
    <Card title={title} padded={false}>
      <DataTable
        caption={title}
        columns={COLUMNS}
        rows={group.accounts ?? []}
        rowKey={(account) => account.id ?? ''}
      />
    </Card>
  )
}

// Conta sistêmica não aparece em seleção de lançamento: só o sistema lança nela.
function EntryContextCell({ account }: { account: LedgerAccount }): ReactNode {
  const label = labelEntryContext(account.entryContext)
  return label ?? <Badge tone="neutral">Sistêmica</Badge>
}
