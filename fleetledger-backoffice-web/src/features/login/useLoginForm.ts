import { useState, type SubmitEvent } from 'react'
import { useLogin } from './useLogin'

const UNEXPECTED_FAILURE_MESSAGE = 'Não foi possível entrar agora. Tente novamente em instantes.'

export interface LoginFormState {
  email: string
  password: string
  setEmail: (email: string) => void
  setPassword: (password: string) => void
  submit: (event: SubmitEvent<HTMLFormElement>) => void
  isPending: boolean
  failureMessage: string | null
}

/**
 * Estado do formulário de login: campos, envio e a mensagem a exibir quando o acesso é recusado.
 * @example const form = useLoginForm()
 */
export function useLoginForm(): LoginFormState {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const login = useLogin()

  const submit = (event: SubmitEvent<HTMLFormElement>): void => {
    event.preventDefault()
    login.mutate({ email, password })
  }

  return {
    email,
    password,
    setEmail,
    setPassword,
    submit,
    isPending: login.isPending,
    failureMessage: failureOf(login),
  }
}

// Falha inesperada (rede, 5xx) não pode se passar por "senha errada".
function failureOf(login: ReturnType<typeof useLogin>): string | null {
  if (login.isError) return UNEXPECTED_FAILURE_MESSAGE
  return login.data?.kind === 'refused' ? login.data.message : null
}
