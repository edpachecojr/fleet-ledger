import type { ButtonHTMLAttributes, ReactNode } from 'react'
import { classNames } from '../classNames'

interface IconButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  label: string
  children: ReactNode
}

// Sem texto visível: o rótulo vai para aria-label, obrigatório.
export function IconButton({ label, className, children, ...props }: IconButtonProps): ReactNode {
  return (
    <button
      type="button"
      aria-label={label}
      title={label}
      className={classNames(
        'grid size-9 place-items-center rounded-md text-text-secondary transition-colors',
        'hover:bg-neutral-soft hover:text-text-primary [&_svg]:size-4',
        className,
      )}
      {...props}
    >
      {children}
    </button>
  )
}
