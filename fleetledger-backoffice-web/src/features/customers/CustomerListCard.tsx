import { Plus, SquareUserRound } from 'lucide-react'
import { useState, type ReactNode } from 'react'
import { RoutePath } from '@/app/routePaths'
import {
  ACTIVITY_FILTER_OPTIONS,
  INITIAL_LIST_QUERY,
  isActivityFilter,
  LIST_PAGE_SIZE,
  type ListQuery,
} from '@/api/listQuery'
import { ButtonLink } from '@/design-system/components/ButtonLink'
import { Card } from '@/design-system/components/Card'
import { DataTable } from '@/design-system/components/DataTable'
import { DataTableToolbar } from '@/design-system/components/DataTableToolbar'
import { EmptyState } from '@/design-system/components/EmptyState'
import { Skeleton } from '@/design-system/components/Skeleton'
import { TablePagination } from '@/design-system/components/TablePagination'
import { ToolbarFilter } from '@/design-system/components/ToolbarFilter'
import { ToolbarSearch } from '@/design-system/components/ToolbarSearch'
import type { CustomerPage } from './customer'
import { CUSTOMER_TABLE_COLUMNS } from './CustomerTableColumns'
import { useCustomers } from './useCustomers'

/** Listagem de clientes (card DataTable): busca por razão social, nome fantasia ou CNPJ. */
export function CustomerListCard(): ReactNode {
  const [query, setQuery] = useState<ListQuery>(INITIAL_LIST_QUERY)
  const customers = useCustomers(query)

  return (
    <Card padded={false}>
      <CustomerListToolbar query={query} onChange={setQuery} />
      {customers.isPending && <Skeleton className="m-6 h-40" />}
      {customers.isError && <CustomersUnavailable />}
      {customers.data && <CustomerTable page={customers.data} query={query} onChange={setQuery} />}
    </Card>
  )
}

interface QueryControlsProps {
  query: ListQuery
  onChange: (query: ListQuery) => void
}

// Mudar a busca ou o filtro volta para a primeira página.
function CustomerListToolbar({ query, onChange }: QueryControlsProps): ReactNode {
  return (
    <DataTableToolbar>
      <ToolbarSearch
        label="Buscar cliente"
        placeholder="Buscar por razão social, nome fantasia ou CNPJ…"
        value={query.search}
        onChange={(search) => {
          onChange({ ...query, search, page: 1 })
        }}
      />
      <ToolbarFilter
        label="Situação"
        options={ACTIVITY_FILTER_OPTIONS}
        value={query.activity}
        onChange={(activity) => {
          if (isActivityFilter(activity)) onChange({ ...query, activity, page: 1 })
        }}
      />
    </DataTableToolbar>
  )
}

function CustomerTable({ page, query, onChange }: QueryControlsProps & { page: CustomerPage }): ReactNode {
  const rows = page.data ?? []
  if (rows.length === 0) return <NoCustomers />

  return (
    <>
      <DataTable
        caption="Clientes"
        columns={CUSTOMER_TABLE_COLUMNS}
        rows={rows}
        rowKey={(customer) => customer.id ?? ''}
      />
      <TablePagination
        page={query.page}
        pageSize={LIST_PAGE_SIZE}
        totalCount={page.totalCount ?? rows.length}
        onPageChange={(nextPage) => {
          onChange({ ...query, page: nextPage })
        }}
      />
    </>
  )
}

function NoCustomers(): ReactNode {
  return (
    <EmptyState
      icon={SquareUserRound}
      title="Nenhum cliente encontrado"
      description="Ajuste a busca ou cadastre o primeiro cliente."
      action={
        <ButtonLink to={RoutePath.newCustomer} icon={<Plus />}>
          Novo cliente
        </ButtonLink>
      }
    />
  )
}

function CustomersUnavailable(): ReactNode {
  return (
    <EmptyState
      icon={SquareUserRound}
      title="Clientes indisponíveis"
      description="Não foi possível carregar os clientes agora. Tente novamente em instantes."
    />
  )
}
