import { Search } from 'lucide-react'
import type { ReactNode } from 'react'
import { classNames } from '../classNames'
import { CONTROL_CLASSES } from './controlClasses'

interface ToolbarSearchProps {
  label: string
  placeholder: string
  value: string
  onChange: (value: string) => void
}

/** Busca da toolbar do DataTable: ícone `Search` à esquerda, até 320px; rótulo só para leitor de tela. */
export function ToolbarSearch({ label, placeholder, value, onChange }: ToolbarSearchProps): ReactNode {
  return (
    <div className="relative w-full max-w-[320px]">
      <Search aria-hidden className="pointer-events-none absolute start-3 top-3 size-4 text-text-secondary" />
      <input
        type="search"
        aria-label={label}
        placeholder={placeholder}
        value={value}
        onChange={(event) => {
          onChange(event.target.value)
        }}
        className={classNames(CONTROL_CLASSES, 'ps-9')}
      />
    </div>
  )
}
