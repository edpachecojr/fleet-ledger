import type { ButtonHTMLAttributes, ReactNode } from 'react'
import { classNames } from '../classNames'

export type ButtonVariant = 'primary' | 'soft' | 'outline' | 'ghost' | 'danger'

const VARIANT_CLASSES: Record<ButtonVariant, string> = {
  primary: 'bg-primary text-on-primary hover:bg-primary-darker hover:shadow-[var(--shadow-primary)]',
  soft: 'bg-primary-lighter text-primary-darker dark:text-primary',
  outline: 'border border-border-input hover:bg-neutral-soft',
  ghost: 'hover:bg-neutral-soft',
  danger: 'bg-error-strong text-white hover:opacity-90',
}

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: ButtonVariant
  icon?: ReactNode
}

/**
 * Classes do botão (card Button), também usadas por links com cara de botão.
 * @example <Link className={buttonClasses('primary')} to="/veiculos/novo">Novo veículo</Link>
 */
export function buttonClasses(variant: ButtonVariant, className?: string): string {
  return classNames(
    'inline-flex h-10 items-center gap-2 rounded-md px-4 text-sm font-semibold transition-colors',
    'disabled:pointer-events-none disabled:bg-neutral-soft disabled:text-text-disabled [&_svg]:size-4',
    VARIANT_CLASSES[variant],
    className,
  )
}

export function Button({ variant = 'primary', icon, className, children, ...props }: ButtonProps): ReactNode {
  return (
    <button type="button" className={buttonClasses(variant, className)} {...props}>
      {icon}
      {children}
    </button>
  )
}
