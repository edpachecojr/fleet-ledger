import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import { applyTheme, browserThemeEnvironment } from './applyTheme'
import { DEFAULT_THEME_SETTINGS, type ThemeSettings } from './themeSettings'

// A chave é lida também pelo script de boot do index.html: mudar nos dois lugares.
export const THEME_SETTINGS_STORAGE_KEY = 'fleetledger-theme-settings'

interface ThemeSettingsState extends ThemeSettings {
  update: (changes: Partial<ThemeSettings>) => void
}

export const useThemeSettings = create<ThemeSettingsState>()(
  persist(
    (set, get) => ({
      ...DEFAULT_THEME_SETTINGS,
      update: (changes) => {
        set(changes)
        applyTheme(get(), browserThemeEnvironment())
      },
    }),
    { name: THEME_SETTINGS_STORAGE_KEY },
  ),
)
