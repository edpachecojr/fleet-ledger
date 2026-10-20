import type { ReactNode } from 'react'
import { classNames } from '../classNames'
import type { SettingsChoice } from './settingsChoices'
import { SettingsOption } from './SettingsOption'

interface SettingsChoiceGroupProps<TValue> {
  choices: readonly SettingsChoice<TValue>[]
  selected: TValue
  onSelect: (value: TValue) => void
}

export function SettingsChoiceGroup<TValue>({
  choices,
  selected,
  onSelect,
}: SettingsChoiceGroupProps<TValue>): ReactNode {
  return (
    <div className={classNames('grid gap-2', choices.length === 2 ? 'grid-cols-2' : 'grid-cols-3')}>
      {choices.map((choice) => (
        <SettingsOption
          key={choice.label}
          icon={choice.icon}
          label={choice.label}
          selected={choice.value === selected}
          onSelect={() => {
            onSelect(choice.value)
          }}
        />
      ))}
    </div>
  )
}
