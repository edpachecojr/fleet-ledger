import { LogIn } from 'lucide-react'
import type { ReactNode } from 'react'
import { Button } from '@/design-system/components/Button'
import { TextField } from '@/design-system/components/TextField'
import { useLoginForm, type LoginFormState } from './useLoginForm'

/** Formulário de e-mail e senha; a mensagem de recusa é a mesma para qualquer credencial errada. */
export function LoginForm(): ReactNode {
  const form = useLoginForm()

  return (
    <form className="grid gap-4" onSubmit={form.submit} noValidate>
      <CredentialFields form={form} />
      {form.failureMessage && <LoginFailure message={form.failureMessage} />}
      <Button type="submit" icon={<LogIn />} disabled={form.isPending} className="w-full justify-center">
        {form.isPending ? 'Entrando…' : 'Entrar'}
      </Button>
    </form>
  )
}

function CredentialFields({ form }: { form: LoginFormState }): ReactNode {
  const invalid = form.failureMessage ? true : undefined

  return (
    <>
      <TextField
        label="E-mail"
        type="email"
        autoComplete="username"
        value={form.email}
        aria-invalid={invalid}
        onChange={(event) => {
          form.setEmail(event.target.value)
        }}
      />
      <TextField
        label="Senha"
        type="password"
        autoComplete="current-password"
        value={form.password}
        aria-invalid={invalid}
        onChange={(event) => {
          form.setPassword(event.target.value)
        }}
      />
    </>
  )
}

function LoginFailure({ message }: { message: string }): ReactNode {
  return (
    <p role="alert" className="rounded-md bg-error-soft px-3 py-2 text-sm text-error-text">
      {message}
    </p>
  )
}
