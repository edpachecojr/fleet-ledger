import { useState } from 'react'

export interface FormValuesState<TValues> {
  values: TValues
  setField: <TField extends keyof TValues>(field: TField, value: TValues[TField]) => void
  reset: (values: TValues) => void
}

/**
 * Estado de um formulário controlado: valores e a troca de um campo por vez.
 * @example const form = useFormValues({ plate: '', model: '' }); form.setField('plate', 'ABC1D23')
 */
export function useFormValues<TValues>(initialValues: TValues): FormValuesState<TValues> {
  const [values, setValues] = useState(initialValues)

  return {
    values,
    setField: (field, value) => {
      setValues((current) => ({ ...current, [field]: value }))
    },
    reset: setValues,
  }
}
