import type { ThemeSettings } from './themeSettings'

export interface ThemeEnvironment {
  root: HTMLElement
  prefersDarkScheme: () => boolean
}

export const browserThemeEnvironment = (): ThemeEnvironment => ({
  root: document.documentElement,
  prefersDarkScheme: () => matchMedia('(prefers-color-scheme: dark)').matches,
})

/**
 * Escreve as preferências como atributos no `<html>`; os tokens CSS reagem a eles.
 * @example applyTheme({ colorMode: 'dark', accent: 'blue', density: 'compact', rtl: false }, browserThemeEnvironment())
 */
export function applyTheme(settings: ThemeSettings, environment: ThemeEnvironment): void {
  const isDark = resolvesToDark(settings, environment)
  const { root } = environment

  root.classList.toggle('dark', isDark)
  root.dataset.theme = isDark ? 'dark' : 'light'
  root.dataset.accent = settings.accent
  root.dataset.density = settings.density
  root.dir = settings.rtl ? 'rtl' : 'ltr'
}

/** Verdadeiro quando o modo escolhido, resolvido contra o sistema, é escuro. */
export function resolvesToDark(settings: ThemeSettings, environment: ThemeEnvironment): boolean {
  if (settings.colorMode === 'system') return environment.prefersDarkScheme()
  return settings.colorMode === 'dark'
}
