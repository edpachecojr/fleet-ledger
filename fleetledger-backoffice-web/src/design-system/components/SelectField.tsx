import { ChevronDown } from 'lucide-react'
import { useId, type ReactNode, type SelectHTMLAttributes } from 'react'
import { classNames } from '../classNames'
import { CONTROL_CLASSES } from './controlClasses'
import { describedByOf, FieldShell } from './FieldShell'

export interface SelectOption {
  value: string
  label: string
}

interface SelectFieldProps extends Omit<SelectHTMLAttributes<HTMLSelectElement>, 'id' | 'children'> {
  label: string
  options: readonly SelectOption[]
  help?: string | undefined
  error?: string | undefined
  labelHidden?: boolean | undefined
}

/**
 * Seleção nativa com a mesma aparência do TextField (card FormControls) e chevron lucide.
 * @example <SelectField label="Ciclo" options={CYCLES} value={cycle} onChange={...} />
 */
export function SelectField({
  label,
  options,
  help,
  error,
  className,
  labelHidden,
  ...selectProps
}: SelectFieldProps): ReactNode {
  const selectId = useId()

  return (
    <FieldShell {...{ label, help, error, className, labelHidden }} controlId={selectId}>
      <div className="relative">
        <select
          id={selectId}
          aria-describedby={describedByOf(selectId, { help, error })}
          aria-invalid={error ? true : undefined}
          className={classNames(CONTROL_CLASSES, 'appearance-none pe-9')}
          {...selectProps}
        >
          <SelectOptions options={options} />
        </select>
        <ChevronDown
          aria-hidden
          className="pointer-events-none absolute end-3 top-3 size-4 text-text-secondary"
        />
      </div>
    </FieldShell>
  )
}

function SelectOptions({ options }: { options: readonly SelectOption[] }): ReactNode {
  return options.map((option) => (
    <option key={option.value} value={option.value}>
      {option.label}
    </option>
  ))
}
