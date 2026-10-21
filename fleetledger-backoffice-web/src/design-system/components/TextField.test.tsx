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

describe('TextField with error', () => {
  it('shows the error instead of the help and marks the input invalid', () => {
    render(<TextField label="Placa" help="Formato Mercosul" error="Placa inválida" />)

    const input = screen.getByRole('textbox', { name: 'Placa' })
    expect(input).toBeInvalid()
    expect(input).toHaveAccessibleDescription('Placa inválida')
    expect(screen.queryByText('Formato Mercosul')).not.toBeInTheDocument()
  })
})
