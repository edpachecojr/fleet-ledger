import { Plus, Truck } from 'lucide-react'
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
import { SelectField } from '@/design-system/components/SelectField'
import { Skeleton } from '@/design-system/components/Skeleton'
import { TablePagination } from '@/design-system/components/TablePagination'
import { ToolbarSearch } from '@/design-system/components/ToolbarSearch'
import { useVehicles } from './useVehicles'
import type { VehiclePage } from './vehicle'
import { VEHICLE_TABLE_COLUMNS } from './VehicleTableColumns'

/** Listagem da frota (card DataTable): busca por placa/modelo, filtro de situação e paginação. */
export function VehicleListCard(): ReactNode {
  const [query, setQuery] = useState<ListQuery>(INITIAL_LIST_QUERY)
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
  query: ListQuery
  onChange: (query: ListQuery) => void
}

// Mudar a busca ou o filtro volta para a primeira página.
function VehicleListToolbar({ query, onChange }: QueryControlsProps): ReactNode {
  return (
    <DataTableToolbar>
      <ToolbarSearch
        label="Buscar veículo"
        placeholder="Buscar por placa ou modelo…"
        value={query.search}
        onChange={(search) => {
          onChange({ ...query, search, page: 1 })
        }}
      />
      <SelectField
        label="Situação"
        labelHidden
        className="w-40"
        options={ACTIVITY_FILTER_OPTIONS}
        value={query.activity}
        onChange={(event) => {
          if (isActivityFilter(event.target.value))
            onChange({ ...query, activity: event.target.value, page: 1 })
        }}
      />
    </DataTableToolbar>
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
