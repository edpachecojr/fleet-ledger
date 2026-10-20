import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { Drawer } from './Drawer'

describe('Drawer', () => {
  it('renders nothing while closed', () => {
    render(
      <Drawer open={false} onClose={() => undefined} side="end" label="Painel">
        conteúdo
      </Drawer>,
    )

    expect(screen.queryByRole('dialog')).not.toBeInTheDocument()
  })

  it.each([
    ['Escape key', async (user: ReturnType<typeof userEvent.setup>) => user.keyboard('{Escape}')],
    [
      'scrim click',
      async (user: ReturnType<typeof userEvent.setup>) =>
        user.click(screen.getByRole('button', { name: 'Fechar' })),
    ],
  ])('calls onClose on %s', async (_, closeWith) => {
    const onClose = vi.fn()
    const user = userEvent.setup()
    render(
      <Drawer open onClose={onClose} side="end" label="Painel">
        conteúdo
      </Drawer>,
    )

    await closeWith(user)

    expect(onClose).toHaveBeenCalledOnce()
  })
})
