import type { ReactNode } from 'react'
import { SelectField, type SelectOption } from './SelectField'

interface ToolbarFilterProps {
  label: string
  options: readonly SelectOption[]
  value: string
  onChange: (value: string) => void
}

/** Filtro da toolbar do DataTable: select compacto com rótulo só para leitor de tela. */
export function ToolbarFilter({ label, options, value, onChange }: ToolbarFilterProps): ReactNode {
  return (
    <SelectField
      label={label}
      labelHidden
      className="w-48"
      options={options}
      value={value}
      onChange={(event) => {
        onChange(event.target.value)
      }}
    />
  )
}
