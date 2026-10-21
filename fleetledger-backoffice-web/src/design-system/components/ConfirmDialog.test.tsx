import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { ConfirmDialog } from './ConfirmDialog'

function renderDialog(handlers: { onCancel?: () => void; onConfirm?: () => void } = {}): void {
  render(
    <ConfirmDialog
      open
      title="Inativar veículo?"
      message="O veículo ABC1D23 sai das seleções."
      confirmLabel="Inativar"
      onCancel={handlers.onCancel ?? (() => undefined)}
      onConfirm={handlers.onConfirm ?? (() => undefined)}
    />,
  )
}

describe('ConfirmDialog', () => {
  it('focuses Cancelar by default', () => {
    renderDialog()

    expect(screen.getByRole('alertdialog', { name: 'Inativar veículo?' })).toBeInTheDocument()
    expect(screen.getByRole('button', { name: 'Cancelar' })).toHaveFocus()
  })

  it('confirms with the action verb', async () => {
    const onConfirm = vi.fn()
    renderDialog({ onConfirm })

    await userEvent.click(screen.getByRole('button', { name: 'Inativar' }))

    expect(onConfirm).toHaveBeenCalledOnce()
  })

  it('cancels on Escape', async () => {
    const onCancel = vi.fn()
    renderDialog({ onCancel })

    await userEvent.keyboard('{Escape}')

    expect(onCancel).toHaveBeenCalledOnce()
  })
})
