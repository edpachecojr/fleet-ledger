import { useId, type ReactNode, type TextareaHTMLAttributes } from 'react'
import { classNames } from '../classNames'
import { CONTROL_CLASSES } from './controlClasses'
import { describedByOf, FieldShell } from './FieldShell'

interface TextAreaFieldProps extends Omit<TextareaHTMLAttributes<HTMLTextAreaElement>, 'id'> {
  label: string
  help?: string | undefined
  error?: string | undefined
}

/** Texto longo com a mesma aparência do TextField (card FormControls, `hz-textarea`). */
export function TextAreaField({
  label,
  help,
  error,
  className,
  ...textareaProps
}: TextAreaFieldProps): ReactNode {
  const textareaId = useId()

  return (
    <FieldShell label={label} controlId={textareaId} help={help} error={error} className={className}>
      <textarea
        id={textareaId}
        rows={3}
        aria-describedby={describedByOf(textareaId, { help, error })}
        aria-invalid={error ? true : undefined}
        className={classNames(CONTROL_CLASSES, 'h-auto min-h-20 py-2')}
        {...textareaProps}
      />
    </FieldShell>
  )
}
