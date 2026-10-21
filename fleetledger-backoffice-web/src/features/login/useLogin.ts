import { useMutation, type UseMutationResult } from '@tanstack/react-query'
import { useApiClient } from '@/api/ApiClientContext'
import { useAdminSession } from '@/session/SessionContext'
import { requestAccessToken, type LoginCredentials, type LoginOutcome } from './requestAccessToken'

/** Envia as credenciais e, se aceitas, inicia a sessão do administrador. */
export function useLogin(): UseMutationResult<LoginOutcome, Error, LoginCredentials> {
  const client = useApiClient()
  const session = useAdminSession()

  return useMutation({
    mutationFn: (credentials: LoginCredentials) => requestAccessToken(client, credentials),
    onSuccess: (outcome) => {
      if (outcome.kind === 'granted') session.start(outcome.accessToken)
    },
  })
}
