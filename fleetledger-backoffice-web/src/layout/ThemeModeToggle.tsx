import { Moon, Sun } from 'lucide-react'
import type { ReactNode } from 'react'
import { IconButton } from '@/design-system/components/IconButton'
import { browserThemeEnvironment, resolvesToDark } from '@/design-system/theme/applyTheme'
import { useThemeSettings } from '@/design-system/theme/themeSettingsStore'

export function ThemeModeToggle(): ReactNode {
  const settings = useThemeSettings()
  const isDark = resolvesToDark(settings, browserThemeEnvironment())

  return (
    <IconButton
      label={isDark ? 'Usar tema claro' : 'Usar tema escuro'}
      onClick={() => {
        settings.update({ colorMode: isDark ? 'light' : 'dark' })
      }}
    >
      {isDark ? <Sun /> : <Moon />}
    </IconButton>
  )
}
