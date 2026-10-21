import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { rangeLabel, TablePagination } from './TablePagination'

describe('rangeLabel', () => {
  it.each([
    [{ page: 1, pageSize: 25, totalCount: 48 }, '1–25 de 48'],
    [{ page: 2, pageSize: 25, totalCount: 48 }, '26–48 de 48'],
    [{ page: 1, pageSize: 25, totalCount: 0 }, '0 de 0'],
  ])('describes %o as "%s"', (state, label) => {
    expect(rangeLabel(state)).toBe(label)
  })
})

describe('TablePagination', () => {
  it('disables previous on the first page and moves to the next one', async () => {
    const onPageChange = vi.fn()
    render(<TablePagination page={1} pageSize={25} totalCount={48} onPageChange={onPageChange} />)

    await userEvent.click(screen.getByRole('button', { name: 'Próxima página' }))

    expect(screen.getByRole('button', { name: 'Página anterior' })).toBeDisabled()
    expect(onPageChange).toHaveBeenCalledWith(2)
  })

  it('disables next on the last page', () => {
    render(<TablePagination page={2} pageSize={25} totalCount={48} onPageChange={() => undefined} />)

    expect(screen.getByRole('button', { name: 'Próxima página' })).toBeDisabled()
  })
})
