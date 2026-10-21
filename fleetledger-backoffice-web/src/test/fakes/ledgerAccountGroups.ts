import type { LedgerAccountGroup } from '@/features/chart-of-accounts/chartOfAccounts'

// Recorte do plano gravado pela migration da API (ChartOfAccountsCatalog).
export const LEDGER_ACCOUNT_GROUPS: LedgerAccountGroup[] = [
  {
    id: 'group-3',
    code: '3',
    name: 'Despesas variáveis de viagem',
    accounts: [
      { id: 'account-301', code: '3.01', name: 'Combustível', groupId: 'group-3', entryContext: 'Trip' },
    ],
  },
  {
    id: 'group-7',
    code: '7',
    name: 'Movimentações não-resultado',
    accounts: [
      {
        id: 'account-701',
        code: '7.01',
        name: 'Adiantamento a motorista',
        groupId: 'group-7',
        entryContext: null,
      },
    ],
  },
]
