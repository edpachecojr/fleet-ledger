import type { ReactNode } from 'react'
import { classNames } from '../classNames'

export type BadgeTone = 'success' | 'warning' | 'info' | 'error' | 'neutral'

const TONE_CLASSES: Record<BadgeTone, string> = {
  success: 'bg-success-soft text-success-text',
  warning: 'bg-warning-soft text-warning-text',
  info: 'bg-info-soft text-info-text',
  error: 'bg-error-soft text-error-text',
  neutral: 'bg-neutral-soft text-text-secondary',
}

interface BadgeProps {
  tone: BadgeTone
  children: ReactNode
}

// O texto é obrigatório: status nunca depende só da cor.
export function Badge({ tone, children }: BadgeProps): ReactNode {
  return (
    <span
      className={classNames(
        'inline-flex h-6 items-center rounded-sm px-2 text-xs font-semibold',
        TONE_CLASSES[tone],
      )}
    >
      {children}
    </span>
  )
}
