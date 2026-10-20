import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { DEFAULT_THEME_SETTINGS } from '../theme/themeSettings'
import { THEME_SETTINGS_STORAGE_KEY, useThemeSettings } from '../theme/themeSettingsStore'
import { ThemeSettingsForm } from './ThemeSettingsForm'

describe('ThemeSettingsForm', () => {
  beforeEach(() => {
    useThemeSettings.setState(DEFAULT_THEME_SETTINGS)
  })

  it('applies the dark mode to the document when selected', async () => {
    const user = userEvent.setup()
    render(<ThemeSettingsForm />)

    await user.click(screen.getByRole('button', { name: 'Escuro' }))

    expect(document.documentElement.dataset.theme).toBe('dark')
    expect(screen.getByRole('button', { name: 'Escuro' })).toHaveAttribute('aria-pressed', 'true')
  })

  it('applies and persists the chosen accent', async () => {
    const user = userEvent.setup()
    render(<ThemeSettingsForm />)

    await user.click(screen.getByRole('button', { name: 'Azul' }))

    expect(document.documentElement.dataset.accent).toBe('blue')
    expect(localStorage.getItem(THEME_SETTINGS_STORAGE_KEY)).toContain('"accent":"blue"')
  })
})
