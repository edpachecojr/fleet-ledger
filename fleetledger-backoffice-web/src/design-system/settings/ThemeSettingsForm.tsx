import type { ReactNode } from 'react'
import { useThemeSettings } from '../theme/themeSettingsStore'
import { AccentSwatches } from './AccentSwatches'
import { COLOR_MODE_CHOICES, DENSITY_CHOICES, DIRECTION_CHOICES } from './settingsChoices'
import { SettingsChoiceGroup } from './SettingsChoiceGroup'
import { SettingsSection } from './SettingsSection'

export function ThemeSettingsForm(): ReactNode {
  return (
    <div className="grid gap-6 overflow-y-auto p-5">
      <ColorModeSetting />
      <AccentSetting />
      <DensitySetting />
      <DirectionSetting />
    </div>
  )
}

function ColorModeSetting(): ReactNode {
  const { colorMode, update } = useThemeSettings()
  return (
    <SettingsSection label="Modo">
      <SettingsChoiceGroup
        choices={COLOR_MODE_CHOICES}
        selected={colorMode}
        onSelect={(value) => {
          update({ colorMode: value })
        }}
      />
    </SettingsSection>
  )
}

function AccentSetting(): ReactNode {
  const { accent, update } = useThemeSettings()
  return (
    <SettingsSection label="Cor">
      <AccentSwatches
        selected={accent}
        onSelect={(value) => {
          update({ accent: value })
        }}
      />
    </SettingsSection>
  )
}

function DensitySetting(): ReactNode {
  const { density, update } = useThemeSettings()
  return (
    <SettingsSection label="Densidade">
      <SettingsChoiceGroup
        choices={DENSITY_CHOICES}
        selected={density}
        onSelect={(value) => {
          update({ density: value })
        }}
      />
    </SettingsSection>
  )
}

function DirectionSetting(): ReactNode {
  const { rtl, update } = useThemeSettings()
  return (
    <SettingsSection label="Direção">
      <SettingsChoiceGroup
        choices={DIRECTION_CHOICES}
        selected={rtl}
        onSelect={(value) => {
          update({ rtl: value })
        }}
      />
    </SettingsSection>
  )
}
