import type { ReactNode } from 'react'
import { classNames } from '../classNames'

export interface FieldShellProps {
  label: string
  controlId: string
  help?: string | undefined
  error?: string | undefined
  className?: string | undefined
  /** Rótulo só para leitor de tela (ex.: filtro na toolbar do DataTable). */
  labelHidden?: boolean | undefined
  children: ReactNode
}

/** Id do texto de ajuda/erro, para o `aria-describedby` do controle. */
export function describedByOf(
  controlId: string,
  { help, error }: Pick<FieldShellProps, 'help' | 'error'>,
): string | undefined {
  return error || help ? `${controlId}-description` : undefined
}

/**
 * Moldura do campo (card FormControls): rótulo 13px/600 → controle → ajuda 12px
 * `text-secondary` ou erro em `error-text`. O erro substitui a ajuda.
 * @example <FieldShell label="Placa" controlId={id} error={error}><input id={id} /></FieldShell>
 */
export function FieldShell({
  label,
  controlId,
  help,
  error,
  className,
  labelHidden,
  children,
}: FieldShellProps): ReactNode {
  const description = error ?? help

  return (
    <div className={classNames('grid content-start gap-1.5', className)}>
      <label
        htmlFor={controlId}
        className={classNames('text-[13px] font-semibold', labelHidden && 'sr-only')}
      >
        {label}
      </label>
      {children}
      {description && (
        <span
          id={describedByOf(controlId, { help, error })}
          className={classNames('text-xs', error ? 'text-error-text' : 'text-text-secondary')}
        >
          {description}
        </span>
      )}
    </div>
  )
}
