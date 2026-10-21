import type { ApiClient } from '@/api/apiClient'
import { ApiRequestError } from '@/api/ApiRequestError'
import type { components } from '@/api/generated/schema'
import type { IssuedAccessToken } from '@/session/storedSession'

export type LoginCredentials = components['schemas']['LoginRequest']

export type LoginOutcome =
  { kind: 'granted'; accessToken: IssuedAccessToken } | { kind: 'refused'; message: string }

const LOGIN_PATH = '/v1/auth/login'
const UNAUTHORIZED_STATUS = 401
const BAD_REQUEST_STATUS = 400

// A API devolve a mesma mensagem para qualquer recusa; este é só o reserva se ela faltar.
export const FALLBACK_REFUSAL_MESSAGE = 'E-mail ou senha inválidos.'
export const MISSING_CREDENTIALS_MESSAGE = 'Informe o e-mail e a senha.'

/**
 * Troca e-mail e senha por um access token. Recusa (401/400) é resultado esperado, não exceção.
 * @example const outcome = await requestAccessToken(client, { email, password })
 */
export async function requestAccessToken(
  client: ApiClient,
  credentials: LoginCredentials,
): Promise<LoginOutcome> {
  const { data, error, response } = await client.POST(LOGIN_PATH, { body: credentials })
  if (data?.data) return { kind: 'granted', accessToken: data.data }
  if (response.status === UNAUTHORIZED_STATUS) return { kind: 'refused', message: refusalMessage(error) }
  if (response.status === BAD_REQUEST_STATUS) return { kind: 'refused', message: MISSING_CREDENTIALS_MESSAGE }

  throw new ApiRequestError({
    method: 'POST',
    path: LOGIN_PATH,
    status: response.status,
    expected: 'ResponseOfIssuedAccessToken (200 com token, 401 recusado ou 400 inválido)',
  })
}

function refusalMessage(body: unknown): string {
  if (typeof body !== 'object' || body === null || !('message' in body)) return FALLBACK_REFUSAL_MESSAGE
  return typeof body.message === 'string' ? body.message : FALLBACK_REFUSAL_MESSAGE
}
