import type { UseMutationResult } from '@tanstack/react-query'
import { useState, type SubmitEvent } from 'react'
import { useFormValues, type FormValuesState } from '@/design-system/useFormValues'
import { VALIDATION_FAILED_MESSAGE, type FieldErrors, type MutationOutcome } from './mutationOutcome'

/** Conversão do formulário digitado para a requisição; número ilegível vira erro do campo. */
export type ParsedForm<TRequest> =
  { kind: 'parsed'; request: TRequest } | { kind: 'invalid'; fieldErrors: FieldErrors }

export interface MutationFormOptions<TValues, TRequest, TValue> {
  initialValues: TValues
  parse: (values: TValues) => ParsedForm<TRequest>
  mutation: UseMutationResult<MutationOutcome<TValue>, Error, TRequest>
  onSaved: (value: TValue) => void
}

export interface MutationFormState<TValues> extends FormValuesState<TValues> {
  fieldErrors: FieldErrors
  failureMessage: string | null
  isPending: boolean
  submit: (event: SubmitEvent<HTMLFormElement>) => void
}

const UNEXPECTED_FAILURE_MESSAGE = 'Não foi possível salvar agora. Tente novamente em instantes.'

/**
 * Ciclo de um formulário de cadastro: valores, envio, erros por campo vindos da conversão
 * ou da API (400) e a mensagem de recusa (404/409). Falha de rede não se passa por recusa.
 * @example const form = useMutationForm({ initialValues, parse, mutation: useCreateVehicle(), onSaved })
 */
export function useMutationForm<TValues, TRequest, TValue>(
  options: MutationFormOptions<TValues, TRequest, TValue>,
): MutationFormState<TValues> {
  const form = useFormValues(options.initialValues)
  const [rejection, setRejection] = useState<{ message: string; fieldErrors: FieldErrors } | null>(null)

  const submit = (event: SubmitEvent<HTMLFormElement>): void => {
    event.preventDefault()
    const parsed = options.parse(form.values)
    if (parsed.kind === 'invalid') {
      setRejection({ message: VALIDATION_FAILED_MESSAGE, fieldErrors: parsed.fieldErrors })
      return
    }
    options.mutation.mutate(parsed.request, {
      onSuccess: (outcome) => {
        setRejection(outcome.kind === 'rejected' ? outcome : null)
        if (outcome.kind === 'saved') options.onSaved(outcome.value)
      },
    })
  }

  return {
    ...form,
    submit,
    fieldErrors: rejection?.fieldErrors ?? {},
    failureMessage: options.mutation.isError ? UNEXPECTED_FAILURE_MESSAGE : (rejection?.message ?? null),
    isPending: options.mutation.isPending,
  }
}

export interface FieldBinding {
  value: string
  error: string | undefined
  onChange: (event: { target: { value: string } }) => void
}

type TextFieldOf<TValues> = {
  [TField in keyof TValues]: TValues[TField] extends string ? TField : never
}[keyof TValues]

/**
 * Liga um campo de texto do formulário a um TextField: valor, erro do campo e troca.
 * @example <TextField label="Placa" {...fieldBinding(form, 'plate')} />
 */
export function fieldBinding<TValues>(
  form: MutationFormState<TValues>,
  field: TextFieldOf<TValues>,
): FieldBinding {
  // `as`: TextFieldOf já restringe o campo aos de tipo string, mas o TypeScript não
  // estreita um tipo indexado por genérico; as conversões só repetem essa garantia.
  return {
    value: form.values[field] as string,
    error: form.fieldErrors[field as string],
    onChange: (event) => {
      form.setField(field, event.target.value as TValues[typeof field])
    },
  }
}
