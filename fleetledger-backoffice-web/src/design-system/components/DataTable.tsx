import type { ReactNode } from 'react'
import { classNames } from '../classNames'

export type DataTableAlign = 'start' | 'end'

export interface DataTableColumn<TRow> {
  key: string
  header: string
  align?: DataTableAlign
  render: (row: TRow) => ReactNode
}

interface DataTableProps<TRow> {
  caption: string
  columns: readonly DataTableColumn<TRow>[]
  rows: readonly TRow[]
  rowKey: (row: TRow) => string
}

// Bordas externas com 24px e as internas com 16px, como no card DataTable.
const CELL_EDGE_CLASSES = 'px-4 first:ps-6 last:pe-6'

const ALIGN_CLASSES: Record<DataTableAlign, string> = {
  start: 'text-start',
  end: 'text-end tabular-nums',
}

/**
 * Tabela do design system (card DataTable): cabeçalho em faixa `surface-hover`, linhas com
 * régua tracejada e hover. Vive rente dentro de um Card (`padded={false}`).
 * @example <DataTable caption="Veículos" columns={columns} rows={vehicles} rowKey={(v) => v.id} />
 */
export function DataTable<TRow>({ caption, columns, rows, rowKey }: DataTableProps<TRow>): ReactNode {
  return (
    <div className="overflow-x-auto">
      <table className="w-full border-collapse text-sm">
        <caption className="sr-only">{caption}</caption>
        <thead>
          <tr>
            {columns.map((column) => (
              <HeaderCell key={column.key} column={column} />
            ))}
          </tr>
        </thead>
        <tbody>
          {rows.map((row) => (
            <BodyRow key={rowKey(row)} row={row} columns={columns} />
          ))}
        </tbody>
      </table>
    </div>
  )
}

function HeaderCell<TRow>({ column }: { column: DataTableColumn<TRow> }): ReactNode {
  return (
    <th
      scope="col"
      className={classNames(
        'bg-surface-hover py-3 text-[13px] font-semibold whitespace-nowrap text-text-secondary',
        CELL_EDGE_CLASSES,
        ALIGN_CLASSES[column.align ?? 'start'],
      )}
    >
      {column.header}
    </th>
  )
}

function BodyRow<TRow>({
  row,
  columns,
}: {
  row: TRow
  columns: readonly DataTableColumn<TRow>[]
}): ReactNode {
  return (
    <tr className="border-b border-dashed border-divider transition-colors last:border-b-0 hover:bg-surface-hover">
      {columns.map((column) => (
        <td
          key={column.key}
          className={classNames(
            'py-[calc(14px*var(--density))]',
            CELL_EDGE_CLASSES,
            ALIGN_CLASSES[column.align ?? 'start'],
          )}
        >
          {column.render(row)}
        </td>
      ))}
    </tr>
  )
}
