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
