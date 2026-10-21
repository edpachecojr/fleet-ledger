import { render, screen } from '@testing-library/react'
import { TextAreaField } from './TextAreaField'

describe('TextAreaField', () => {
  it('associates the label and describes the textarea with the help', () => {
    render(<TextAreaField label="Dados bancários" help="Opcional." />)

    expect(screen.getByRole('textbox', { name: 'Dados bancários' })).toHaveAccessibleDescription('Opcional.')
  })
})
