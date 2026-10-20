import type { ReactNode } from 'react'
import { classNames } from '../classNames'

interface CardProps {
  title?: string
  actions?: ReactNode
  padded?: boolean
  className?: string
  children: ReactNode
}

/** Único container de conteúdo: surface, raio 16px, sombra, sem borda. */
export function Card({ title, actions, padded = true, className, children }: CardProps): ReactNode {
  return (
    <section className={classNames('rounded-2xl bg-surface shadow-[var(--shadow-card)]', className)}>
      {title && <CardHeader title={title} actions={actions} />}
      <div className={classNames(padded && 'p-[calc(24px*var(--density))]')}>{children}</div>
    </section>
  )
}

function CardHeader({ title, actions }: { title: string; actions: ReactNode }): ReactNode {
  return (
    <header className="flex min-h-14 items-center justify-between gap-3 border-b border-divider px-6 py-4">
      <h2 className="text-base font-semibold">{title}</h2>
      {actions}
    </header>
  )
}
