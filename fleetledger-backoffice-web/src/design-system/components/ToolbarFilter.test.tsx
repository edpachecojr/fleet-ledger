import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { ToolbarFilter } from './ToolbarFilter'

describe('ToolbarFilter', () => {
  it('reports the chosen option value', async () => {
    const onChange = vi.fn()
    render(
      <ToolbarFilter
        label="Situação"
        options={[
          { value: 'active', label: 'Ativos' },
          { value: 'all', label: 'Todos' },
        ]}
        value="active"
        onChange={onChange}
      />,
    )

    await userEvent.selectOptions(screen.getByRole('combobox', { name: 'Situação' }), 'Todos')

    expect(onChange).toHaveBeenCalledWith('all')
  })
})
