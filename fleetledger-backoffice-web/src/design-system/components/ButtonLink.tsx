import type { ReactNode } from 'react'
import { Link } from 'react-router'
import { buttonClasses, type ButtonVariant } from './Button'

interface ButtonLinkProps {
  to: string
  variant?: ButtonVariant
  icon?: ReactNode
  children: ReactNode
}

/** Navegação com aparência de botão (ex.: "Novo veículo", "Cancelar" de um formulário). */
export function ButtonLink({ to, variant = 'primary', icon, children }: ButtonLinkProps): ReactNode {
  return (
    <Link to={to} className={buttonClasses(variant)}>
      {icon}
      {children}
    </Link>
  )
}
