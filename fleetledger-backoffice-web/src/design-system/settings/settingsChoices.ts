import {
  AlignLeft,
  AlignRight,
  Monitor,
  Moon,
  Rows3,
  Rows4,
  Square,
  Sun,
  type LucideIcon,
} from 'lucide-react'
import type { ColorMode, Density } from '../theme/themeSettings'

export interface SettingsChoice<TValue> {
  value: TValue
  label: string
  icon: LucideIcon
}

export const COLOR_MODE_CHOICES: readonly SettingsChoice<ColorMode>[] = [
  { value: 'light', label: 'Claro', icon: Sun },
  { value: 'dark', label: 'Escuro', icon: Moon },
  { value: 'system', label: 'Sistema', icon: Monitor },
]

export const DENSITY_CHOICES: readonly SettingsChoice<Density>[] = [
  { value: 'compact', label: 'Compacto', icon: Rows4 },
  { value: 'default', label: 'Padrão', icon: Rows3 },
  { value: 'spacious', label: 'Espaçoso', icon: Square },
]

export const DIRECTION_CHOICES: readonly SettingsChoice<boolean>[] = [
  { value: false, label: 'LTR', icon: AlignLeft },
  { value: true, label: 'RTL', icon: AlignRight },
]
