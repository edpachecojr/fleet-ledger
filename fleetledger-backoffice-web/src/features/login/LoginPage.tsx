import type { ReactNode } from 'react'
import { Navigate, useLocation } from 'react-router'
import { PRODUCT_NAME } from '@/app/product'
import { Card } from '@/design-system/components/Card'
import { useIsSessionActive } from '@/session/SessionContext'
import { LoginForm } from './LoginForm'
import { redirectTargetAfterLogin } from './loginRedirect'

/** Única tela pública: com sessão ativa, segue direto para a rota que pediu login. */
export function LoginPage(): ReactNode {
  const isActive = useIsSessionActive()
  // O React Router tipa o estado de navegação como any: tratado como desconhecido e validado.
  const navigationState: unknown = useLocation().state
  if (isActive) return <Navigate to={redirectTargetAfterLogin(navigationState)} replace />

  return (
    <main className="grid min-h-screen place-items-center bg-bg p-4">
      <div className="grid w-full max-w-sm gap-6">
        <div className="flex items-center justify-center gap-2.5 text-xl font-bold">
          <span className="grid size-9 place-items-center rounded-md bg-primary text-on-primary">F</span>
          {PRODUCT_NAME}
        </div>
        <Card>
          <div className="mb-6">
            <h1 className="text-2xl font-bold">Entrar</h1>
            <p className="mt-1 text-sm text-text-secondary">Acesso do administrador da frota.</p>
          </div>
          <LoginForm />
        </Card>
      </div>
    </main>
  )
}
