import { render, screen } from '@testing-library/react'
import { TextField } from './TextField'

describe('TextField', () => {
  it('associates the label with the input', () => {
    render(<TextField label="E-mail" />)

    expect(screen.getByRole('textbox', { name: 'E-mail' })).toBeInTheDocument()
  })

  it('describes the input with the help text', () => {
    render(<TextField label="Placa" help="Formato Mercosul" />)

    expect(screen.getByRole('textbox', { name: 'Placa' })).toHaveAccessibleDescription('Formato Mercosul')
  })
})
