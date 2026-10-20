import type { LucideIcon } from 'lucide-react'
import type { ReactNode } from 'react'

interface EmptyStateProps {
  icon: LucideIcon
  title: string
  description?: string
  action?: ReactNode
}

export function EmptyState({ icon: Icon, title, description, action }: EmptyStateProps): ReactNode {
  return (
    <div className="grid justify-items-center gap-3 p-10 text-center">
      <span className="grid size-16 place-items-center rounded-full bg-neutral-soft text-text-disabled">
        <Icon className="size-8" aria-hidden />
      </span>
      <h3 className="text-base font-semibold">{title}</h3>
      {description && <p className="max-w-[360px] text-text-secondary">{description}</p>}
      {action}
    </div>
  )
}
