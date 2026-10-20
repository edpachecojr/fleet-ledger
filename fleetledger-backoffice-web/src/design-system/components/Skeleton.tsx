import type { ReactNode } from 'react'
import { classNames } from '../classNames'

export function Skeleton({ className }: { className?: string }): ReactNode {
  return (
    <span aria-hidden className={classNames('block animate-pulse rounded-md bg-surface-hover', className)} />
  )
}
