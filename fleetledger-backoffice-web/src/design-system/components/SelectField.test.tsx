import { render, screen } from '@testing-library/react'
import { SelectField } from './SelectField'

const OPTIONS = [
  { value: 'Weekly', label: 'Semanal' },
  { value: 'Monthly', label: 'Mensal' },
]

describe('SelectField', () => {
  it('associates the label with the select and lists the options', () => {
    render(<SelectField label="Ciclo" options={OPTIONS} />)

    const select = screen.getByRole('combobox', { name: 'Ciclo' })
    expect(select).toHaveDisplayValue('Semanal')
    expect(screen.getAllByRole('option')).toHaveLength(2)
  })

  it('marks the select invalid and describes it with the error', () => {
    render(<SelectField label="Ciclo" options={OPTIONS} error="Ciclo é obrigatório" />)

    const select = screen.getByRole('combobox', { name: 'Ciclo' })
    expect(select).toBeInvalid()
    expect(select).toHaveAccessibleDescription('Ciclo é obrigatório')
  })
})
