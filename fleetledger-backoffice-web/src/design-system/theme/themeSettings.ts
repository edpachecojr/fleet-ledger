export type ColorMode = 'light' | 'dark' | 'system'
export type Accent = 'teal' | 'blue' | 'purple' | 'orange' | 'rose' | 'emerald'
export type Density = 'compact' | 'default' | 'spacious'

export interface ThemeSettings {
  colorMode: ColorMode
  accent: Accent
  density: Density
  rtl: boolean
}

export const DEFAULT_THEME_SETTINGS: ThemeSettings = {
  colorMode: 'system',
  accent: 'teal',
  density: 'default',
  rtl: false,
}

// Amostras exibidas no seletor de cor; o tema real é recalculado em OKLCh a partir de --accent-h.
export const ACCENT_SWATCHES: readonly { accent: Accent; swatch: string; label: string }[] = [
  { accent: 'teal', swatch: '#14b8a6', label: 'Verde-azulado' },
  { accent: 'blue', swatch: '#3b82f6', label: 'Azul' },
  { accent: 'purple', swatch: '#8b5cf6', label: 'Roxo' },
  { accent: 'orange', swatch: '#f97316', label: 'Laranja' },
  { accent: 'rose', swatch: '#f43f5e', label: 'Rosa' },
  { accent: 'emerald', swatch: '#10b981', label: 'Esmeralda' },
]
