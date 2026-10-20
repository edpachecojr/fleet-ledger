import type { LucideIcon } from 'lucide-react'
import type { ReactNode } from 'react'
import { classNames } from '../classNames'

interface SettingsOptionProps {
  icon: LucideIcon
  label: string
  selected: boolean
  onSelect: () => void
}

export function SettingsOption({ icon: Icon, label, selected, onSelect }: SettingsOptionProps): ReactNode {
  return (
    <button
      type="button"
      aria-pressed={selected}
      onClick={onSelect}
      className={classNames(
        'grid h-[68px] place-content-center justify-items-center gap-1.5 rounded-lg text-[13px] font-semibold transition-colors',
        selected ? 'border-2 border-primary bg-primary/10' : 'border border-divider hover:bg-neutral-soft',
      )}
    >
      <Icon className="size-[18px]" aria-hidden />
      {label}
    </button>
  )
}
