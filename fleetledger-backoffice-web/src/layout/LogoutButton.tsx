import { LogOut } from 'lucide-react'
import type { ReactNode } from 'react'
import { IconButton } from '@/design-system/components/IconButton'
import { useAdminSession } from '@/session/SessionContext'

// Encerrar a sessão basta: RequireSession limpa o cache e leva ao login.
export function LogoutButton(): ReactNode {
  const session = useAdminSession()

  return (
    <IconButton
      label="Sair"
      onClick={() => {
        session.end()
      }}
    >
      <LogOut />
    </IconButton>
  )
}
