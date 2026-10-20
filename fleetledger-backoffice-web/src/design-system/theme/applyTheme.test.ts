import { applyTheme, type ThemeEnvironment } from './applyTheme'
import { DEFAULT_THEME_SETTINGS, type ThemeSettings } from './themeSettings'

function environment(prefersDark: boolean): ThemeEnvironment {
  return { root: document.createElement('html'), prefersDarkScheme: () => prefersDark }
}

function apply(changes: Partial<ThemeSettings>, prefersDark = false): HTMLElement {
  const env = environment(prefersDark)
  applyTheme({ ...DEFAULT_THEME_SETTINGS, ...changes }, env)
  return env.root
}

describe('applyTheme', () => {
  it.each([
    ['dark', false, 'dark'],
    ['light', true, 'light'],
    ['system', true, 'dark'],
    ['system', false, 'light'],
  ] as const)('resolves %s mode (system prefers dark: %s) to %s', (colorMode, prefersDark, expected) => {
    const root = apply({ colorMode }, prefersDark)

    expect(root.dataset.theme).toBe(expected)
    expect(root.classList.contains('dark')).toBe(expected === 'dark')
  })

  it('writes accent, density and direction as attributes', () => {
    const root = apply({ accent: 'blue', density: 'compact', rtl: true })

    expect(root.dataset.accent).toBe('blue')
    expect(root.dataset.density).toBe('compact')
    expect(root.dir).toBe('rtl')
  })
})
