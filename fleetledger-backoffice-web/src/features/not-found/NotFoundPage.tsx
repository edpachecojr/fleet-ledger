import { MapPinOff } from 'lucide-react'
import type { ReactNode } from 'react'
import { Link } from 'react-router'
import { RoutePath } from '@/app/routePaths'
import { Card } from '@/design-system/components/Card'
import { EmptyState } from '@/design-system/components/EmptyState'

export function NotFoundPage(): ReactNode {
  return (
    <Card>
      <EmptyState
        icon={MapPinOff}
        title="Página não encontrada"
        description="O endereço acessado não existe ou foi movido."
        action={
          <Link to={RoutePath.overview} className="font-semibold text-primary hover:underline">
            Voltar para a visão geral
          </Link>
        }
      />
    </Card>
  )
}
