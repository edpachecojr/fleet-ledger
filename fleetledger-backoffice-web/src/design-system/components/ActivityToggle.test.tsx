import { render, screen, within } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { ActivityToggle, type ActivityToggleProps } from './ActivityToggle'

function renderToggle(overrides: Partial<ActivityToggleProps> = {}): ActivityToggleProps['onChange'] {
  const onChange = overrides.onChange ?? vi.fn()
  render(
    <ActivityToggle
      isActive
      confirmTitle="Inativar cliente?"
      consequence="O cliente sai das seleções."
      pending={false}
      rejection={null}
      {...overrides}
      onChange={onChange}
    />,
  )
  return onChange
}

describe('ActivityToggle', () => {
  it('deactivates only after confirmation', async () => {
    const onChange = renderToggle()

    await userEvent.click(screen.getByRole('button', { name: 'Inativar' }))
    expect(onChange).not.toHaveBeenCalled()
    await userEvent.click(
      within(screen.getByRole('alertdialog', { name: 'Inativar cliente?' })).getByRole('button', {
        name: 'Inativar',
      }),
    )

    expect(onChange).toHaveBeenCalledWith(true, expect.any(Function))
  })

  it('reactivates directly when inactive', async () => {
    const onChange = renderToggle({ isActive: false })

    await userEvent.click(screen.getByRole('button', { name: 'Reativar' }))

    expect(onChange).toHaveBeenCalledWith(false, expect.any(Function))
  })

  it('shows the refusal message', () => {
    renderToggle({ rejection: 'Cliente não encontrado.' })

    expect(screen.getByRole('alert')).toHaveTextContent('Cliente não encontrado.')
  })
})
