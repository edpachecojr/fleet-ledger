import { ApiRequestError } from './ApiRequestError'

/** Mensagem por campo do formulário, com o nome em camelCase como no contrato. */
export type FieldErrors = Readonly<Partial<Record<string, string>>>

export interface MutationRejection {
  kind: 'rejected'
  message: string
  fieldErrors: FieldErrors
}

export type MutationOutcome<TValue> = { kind: 'saved'; value: TValue } | MutationRejection

export const VALIDATION_FAILED_MESSAGE = 'Revise os campos destacados.'
const FALLBACK_REJECTION_MESSAGE = 'A operação foi recusada.'

const BAD_REQUEST_STATUS = 400
// Regra de negócio recusada (não encontrado, duplicado, transição inválida): é resposta
// esperada com mensagem para o usuário, não falha de comunicação.
const BUSINESS_REJECTION_STATUSES = new Set([404, 409, 422])

/**
 * Transforma a recusa da API (400 ValidationProblem, 404/409/422 Response) em resultado
 * para o formulário; `null` quando o status não é uma recusa esperada.
 * @example rejectionOf(409, { message: 'Já existe um veículo com a placa ABC1D23.' })
 */
export function rejectionOf(status: number, body: unknown): MutationRejection | null {
  if (status === BAD_REQUEST_STATUS)
    return { kind: 'rejected', message: VALIDATION_FAILED_MESSAGE, fieldErrors: fieldErrorsOf(body) }
  if (BUSINESS_REJECTION_STATUSES.has(status))
    return { kind: 'rejected', message: messageOf(body) ?? FALLBACK_REJECTION_MESSAGE, fieldErrors: {} }
  return null
}

function messageOf(body: unknown): string | null {
  if (!isRecord(body)) return null
  return typeof body.message === 'string' ? body.message : null
}

// A API nomeia o campo como a propriedade C# (PascalCase); o formulário usa camelCase.
function fieldErrorsOf(body: unknown): FieldErrors {
  if (!isRecord(body) || !isRecord(body.errors)) return {}
  const entries = Object.entries(body.errors).map(([field, messages]) => [
    toCamelCase(field),
    firstMessage(messages),
  ])
  return Object.fromEntries(entries.filter(([, message]) => message !== undefined)) as FieldErrors
}

function firstMessage(messages: unknown): string | undefined {
  return Array.isArray(messages) && typeof messages[0] === 'string' ? messages[0] : undefined
}

function toCamelCase(field: string): string {
  return field.charAt(0).toLowerCase() + field.slice(1)
}

function isRecord(value: unknown): value is Record<string, unknown> {
  return typeof value === 'object' && value !== null
}

export interface ApiCallResult<TValue> {
  data?: { data?: TValue | null } | undefined
  error?: unknown
  response: Response
}

export interface ApiCallDescription {
  method: string
  path: string
  expected: string
}

/**
 * Resultado de uma escrita na API: o valor salvo, a recusa esperada ou `ApiRequestError`
 * quando a resposta foge do contrato (5xx, proxy sem API).
 * @example return outcomeOf(await client.POST('/v1/vehicles', { body }), { method: 'POST', path, expected })
 */
export function outcomeOf<TValue>(
  result: ApiCallResult<TValue>,
  description: ApiCallDescription,
): MutationOutcome<TValue> {
  if (result.data?.data) return { kind: 'saved', value: result.data.data }
  const rejection = rejectionOf(result.response.status, result.error)
  if (rejection) return rejection

  throw new ApiRequestError({ ...description, status: result.response.status })
}
