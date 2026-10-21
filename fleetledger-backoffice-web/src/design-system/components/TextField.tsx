import { useId, type InputHTMLAttributes, type ReactNode } from 'react'
import { classNames } from '../classNames'

interface TextFieldProps extends Omit<InputHTMLAttributes<HTMLInputElement>, 'id'> {
  label: string
  help?: string
}

/**
 * Campo do design system (card FormControls): rótulo 13px/600, controle de 40px com
 * borda `border-input`, foco com borda e anel `primary`, texto de ajuda opcional.
 * @example <TextField label="E-mail" type="email" autoComplete="username" />
 */
export function TextField({ label, help, className, ...inputProps }: TextFieldProps): ReactNode {
  const inputId = useId()
  const helpId = `${inputId}-help`

  return (
    <div className={classNames('grid gap-1.5', className)}>
      <label htmlFor={inputId} className="text-[13px] font-semibold">
        {label}
      </label>
      <input
        id={inputId}
        aria-describedby={help ? helpId : undefined}
        className={classNames(
          'h-10 rounded-md border border-border-input bg-transparent px-3 text-sm transition-colors outline-none',
          'placeholder:text-text-disabled hover:border-text-primary',
          'focus:border-primary focus:ring-1 focus:ring-primary',
          'disabled:bg-neutral-soft aria-invalid:border-error',
        )}
        {...inputProps}
      />
      {help && (
        <span id={helpId} className="text-xs text-text-secondary">
          {help}
        </span>
      )}
    </div>
  )
}
