import { Plus, Truck } from 'lucide-react'
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
import { useVehicles } from './useVehicles'
import {
  isVehicleStatusFilter,
  VEHICLE_STATUS_FILTER_OPTIONS,
  type VehicleListQuery,
  type VehiclePage,
} from './vehicle'
import { VEHICLE_TABLE_COLUMNS } from './VehicleTableColumns'

/** Listagem da frota (card DataTable): busca por placa/modelo, filtros de situação e status, paginação. */
export function VehicleListCard(): ReactNode {
  const [query, setQuery] = useState<VehicleListQuery>({ ...INITIAL_LIST_QUERY, status: 'all' })
  const vehicles = useVehicles(query)

  return (
    <Card padded={false}>
      <VehicleListToolbar query={query} onChange={setQuery} />
      {vehicles.isPending && <Skeleton className="m-6 h-40" />}
      {vehicles.isError && <VehiclesUnavailable />}
      {vehicles.data && <VehicleTable page={vehicles.data} query={query} onChange={setQuery} />}
    </Card>
  )
}

interface QueryControlsProps {
  query: VehicleListQuery
  onChange: (query: VehicleListQuery) => void
}

type RefineVehicleQuery = (change: Partial<VehicleListQuery>) => void

// Mudar a busca ou um filtro volta para a primeira página.
function VehicleListToolbar({ query, onChange }: QueryControlsProps): ReactNode {
  const refine: RefineVehicleQuery = (change) => {
    onChange({ ...query, ...change, page: 1 })
  }

  return (
    <DataTableToolbar>
      <ToolbarSearch
        label="Buscar veículo"
        placeholder="Buscar por placa ou modelo…"
        value={query.search}
        onChange={(search) => {
          refine({ search })
        }}
      />
      <VehicleListFilters query={query} refine={refine} />
    </DataTableToolbar>
  )
}

function VehicleListFilters({
  query,
  refine,
}: {
  query: VehicleListQuery
  refine: RefineVehicleQuery
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
        label="Status"
        options={VEHICLE_STATUS_FILTER_OPTIONS}
        value={query.status}
        onChange={(status) => {
          if (isVehicleStatusFilter(status)) refine({ status })
        }}
      />
    </>
  )
}

function VehicleTable({ page, query, onChange }: QueryControlsProps & { page: VehiclePage }): ReactNode {
  const rows = page.data ?? []
  if (rows.length === 0) return <NoVehicles />

  return (
    <>
      <DataTable
        caption="Veículos"
        columns={VEHICLE_TABLE_COLUMNS}
        rows={rows}
        rowKey={(vehicle) => vehicle.id ?? ''}
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

function NoVehicles(): ReactNode {
  return (
    <EmptyState
      icon={Truck}
      title="Nenhum veículo encontrado"
      description="Ajuste a busca ou cadastre o primeiro veículo da frota."
      action={
        <ButtonLink to={RoutePath.newVehicle} icon={<Plus />}>
          Novo veículo
        </ButtonLink>
      }
    />
  )
}

function VehiclesUnavailable(): ReactNode {
  return (
    <EmptyState
      icon={Truck}
      title="Veículos indisponíveis"
      description="Não foi possível carregar a frota agora. Tente novamente em instantes."
    />
  )
}
