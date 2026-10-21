import { render, screen, within } from '@testing-library/react'
import { DataTable, type DataTableColumn } from './DataTable'

interface Fruit {
  id: string
  name: string
  stock: number
}

const COLUMNS: DataTableColumn<Fruit>[] = [
  { key: 'name', header: 'Nome', render: (fruit) => fruit.name },
  { key: 'stock', header: 'Estoque', align: 'end', render: (fruit) => fruit.stock },
]

const FRUITS: Fruit[] = [
  { id: '1', name: 'Maçã', stock: 12 },
  { id: '2', name: 'Pera', stock: 3 },
]

describe('DataTable', () => {
  it('renders a header cell per column and a row per item', () => {
    render(<DataTable caption="Frutas" columns={COLUMNS} rows={FRUITS} rowKey={(fruit) => fruit.id} />)

    const table = screen.getByRole('table', { name: 'Frutas' })
    expect(
      within(table)
        .getAllByRole('columnheader')
        .map((cell) => cell.textContent),
    ).toEqual(['Nome', 'Estoque'])
    expect(within(table).getAllByRole('row')).toHaveLength(3)
  })

  it('right-aligns numeric columns with tabular figures', () => {
    render(<DataTable caption="Frutas" columns={COLUMNS} rows={FRUITS} rowKey={(fruit) => fruit.id} />)

    expect(screen.getByRole('cell', { name: '12' })).toHaveClass('text-end', 'tabular-nums')
  })

  it('hides secondary columns below 1024px and applies fixed widths', () => {
    const columns: DataTableColumn<Fruit>[] = [
      { key: 'name', header: 'Nome', widthClass: 'w-28', render: (fruit) => fruit.name },
      { key: 'stock', header: 'Estoque', secondary: true, render: (fruit) => fruit.stock },
    ]
    render(<DataTable caption="Frutas" columns={columns} rows={FRUITS} rowKey={(fruit) => fruit.id} />)

    expect(screen.getByRole('columnheader', { name: 'Estoque' })).toHaveClass('hidden', 'lg:table-cell')
    expect(screen.getByRole('columnheader', { name: 'Nome' })).toHaveClass('w-28')
  })
})
