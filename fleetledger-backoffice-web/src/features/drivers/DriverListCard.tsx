import { IdCard, Plus } from 'lucide-react'
import { useState, type ReactNode } from 'react'
import { RoutePath } from '@/app/routePaths'
import {
  ACTIVITY_FILTER_OPTIONS,
  INITIAL_LIST_QUERY,
  isActivityFilter,
  LIST_PAGE_SIZE,
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
import {
  DOCUMENT_STATUS_FILTER_OPTIONS,
  isDocumentStatusFilter,
  type DriverListQuery,
  type DriverPage,
} from './driver'
import { DRIVER_TABLE_COLUMNS } from './DriverTableColumns'
import { useDrivers } from './useDrivers'

/** Listagem de motoristas (card DataTable): busca por nome/CPF, situação e situação documental. */
export function DriverListCard(): ReactNode {
  const [query, setQuery] = useState<DriverListQuery>({ ...INITIAL_LIST_QUERY, documentStatus: 'all' })
  const drivers = useDrivers(query)

  return (
    <Card padded={false}>
      <DriverListToolbar query={query} onChange={setQuery} />
      {drivers.isPending && <Skeleton className="m-6 h-40" />}
      {drivers.isError && <DriversUnavailable />}
      {drivers.data && <DriverTable page={drivers.data} query={query} onChange={setQuery} />}
    </Card>
  )
}

interface QueryControlsProps {
  query: DriverListQuery
  onChange: (query: DriverListQuery) => void
}

type RefineDriverQuery = (change: Partial<DriverListQuery>) => void

// Mudar a busca ou um filtro volta para a primeira página.
function DriverListToolbar({ query, onChange }: QueryControlsProps): ReactNode {
  const refine: RefineDriverQuery = (change) => {
    onChange({ ...query, ...change, page: 1 })
  }

  return (
    <DataTableToolbar>
      <ToolbarSearch
        label="Buscar motorista"
        placeholder="Buscar por nome ou CPF…"
        value={query.search}
        onChange={(search) => {
          refine({ search })
        }}
      />
      <DriverListFilters query={query} refine={refine} />
    </DataTableToolbar>
  )
}

function DriverListFilters({
  query,
  refine,
}: {
  query: DriverListQuery
  refine: RefineDriverQuery
}): ReactNode {
  return (
    <>
      <ToolbarFilter
        label="Situação"
        options={ACTIVITY_FILTER_OPTIONS}
        value={query.activity}
        onChange={(activity) => {
          if (isActivityFilter(activity)) refine({ activity })
        }}
      />
      <ToolbarFilter
        label="Documentos"
        options={DOCUMENT_STATUS_FILTER_OPTIONS}
        value={query.documentStatus}
        onChange={(documentStatus) => {
          if (isDocumentStatusFilter(documentStatus)) refine({ documentStatus })
        }}
      />
    </>
  )
}

function DriverTable({ page, query, onChange }: QueryControlsProps & { page: DriverPage }): ReactNode {
  const rows = page.data ?? []
  if (rows.length === 0) return <NoDrivers />

  return (
    <>
      <DataTable
        caption="Motoristas"
        columns={DRIVER_TABLE_COLUMNS}
        rows={rows}
        rowKey={(driver) => driver.id ?? ''}
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

function NoDrivers(): ReactNode {
  return (
    <EmptyState
      icon={IdCard}
      title="Nenhum motorista encontrado"
      description="Ajuste a busca ou cadastre o primeiro motorista."
      action={
        <ButtonLink to={RoutePath.newDriver} icon={<Plus />}>
          Novo motorista
        </ButtonLink>
      }
    />
  )
}

function DriversUnavailable(): ReactNode {
  return (
    <EmptyState
      icon={IdCard}
      title="Motoristas indisponíveis"
      description="Não foi possível carregar os motoristas agora. Tente novamente em instantes."
    />
  )
}
