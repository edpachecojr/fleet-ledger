import { useId, type InputHTMLAttributes, type ReactNode } from 'react'
import { CONTROL_CLASSES } from './controlClasses'
import { describedByOf, FieldShell } from './FieldShell'

interface TextFieldProps extends Omit<InputHTMLAttributes<HTMLInputElement>, 'id'> {
  label: string
  help?: string | undefined
  error?: string | undefined
}

/**
 * Campo do design system (card FormControls): rótulo 13px/600, controle de 40px com
 * borda `border-input`, foco com borda e anel `primary`, texto de ajuda ou erro opcional.
 * @example <TextField label="E-mail" type="email" autoComplete="username" />
 */
export function TextField({ label, help, error, className, ...inputProps }: TextFieldProps): ReactNode {
  const inputId = useId()

  return (
    <FieldShell label={label} controlId={inputId} help={help} error={error} className={className}>
      <input
        id={inputId}
        aria-describedby={describedByOf(inputId, { help, error })}
        aria-invalid={error ? true : undefined}
        className={CONTROL_CLASSES}
        {...inputProps}
      />
    </FieldShell>
  )
}
