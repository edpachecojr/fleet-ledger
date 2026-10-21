import { ChevronLeft, ChevronRight } from 'lucide-react'
import type { ReactNode } from 'react'
import { IconButton } from './IconButton'

interface TablePaginationProps {
  page: number
  pageSize: number
  totalCount: number
  onPageChange: (page: number) => void
}

/**
 * Faixa "1–25 de 48" (card DataTable). O intervalo é calculado aqui para todas as listas.
 * @example rangeLabel({ page: 2, pageSize: 25, totalCount: 48 }) // '26–48 de 48'
 */
export function rangeLabel({
  page,
  pageSize,
  totalCount,
}: Omit<TablePaginationProps, 'onPageChange'>): string {
  if (totalCount === 0) return '0 de 0'
  const first = (page - 1) * pageSize + 1
  const last = Math.min(page * pageSize, totalCount)
  return `${first}–${last} de ${totalCount}`
}

export function TablePagination({
  page,
  pageSize,
  totalCount,
  onPageChange,
}: TablePaginationProps): ReactNode {
  return (
    <div className="flex items-center justify-end gap-4 border-t border-divider px-6 py-3 text-[13px] text-text-secondary">
      <span>Linhas por página: {pageSize}</span>
      <span className="tabular-nums">{rangeLabel({ page, pageSize, totalCount })}</span>
      <PageButtons
        page={page}
        lastPage={Math.max(1, Math.ceil(totalCount / pageSize))}
        onPageChange={onPageChange}
      />
    </div>
  )
}

interface PageButtonsProps {
  page: number
  lastPage: number
  onPageChange: (page: number) => void
}

function PageButtons({ page, lastPage, onPageChange }: PageButtonsProps): ReactNode {
  return (
    <span className="flex gap-1">
      <IconButton
        label="Página anterior"
        disabled={page <= 1}
        onClick={() => {
          onPageChange(page - 1)
        }}
      >
        <ChevronLeft className="rtl:rotate-180" />
      </IconButton>
      <IconButton
        label="Próxima página"
        disabled={page >= lastPage}
        onClick={() => {
          onPageChange(page + 1)
        }}
      >
        <ChevronRight className="rtl:rotate-180" />
      </IconButton>
    </span>
  )
}
