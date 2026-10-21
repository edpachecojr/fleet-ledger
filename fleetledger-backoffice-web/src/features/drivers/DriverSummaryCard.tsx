import type { ReactNode } from 'react'
import { presentActivity } from '@/api/listQuery'
import { Badge } from '@/design-system/components/Badge'
import { Card } from '@/design-system/components/Card'
import { SummaryList, SummaryRow } from '@/design-system/components/SummaryList'
import { formatDateOnly } from '@/design-system/dateFormat'
import { formatCpf } from '@/design-system/documentFormat'
import { formatPercent } from '@/design-system/numberFormat'
import { presentDocumentStatus, type Driver } from './driver'

/** Documentos e comissão do motorista, com a situação documental de hoje em destaque. */
export function DriverSummaryCard({ driver }: { driver: Driver }): ReactNode {
  const documents = presentDocumentStatus(driver.documentStatus)
  const activity = presentActivity(driver.isActive)

  return (
    <Card title="Documentos">
      <SummaryList>
        <SummaryRow term="Situação documental">
          <Badge tone={documents.tone}>{documents.label}</Badge>
        </SummaryRow>
        <SummaryRow term="Validade da CNH">{formatDateOnly(driver.cnhExpiresOn)}</SummaryRow>
        <SummaryRow term="Validade do toxicológico">
          {formatDateOnly(driver.toxicologyExamExpiresOn)}
        </SummaryRow>
        <SummaryRow term="CNH">
          {driver.cnhNumber} · {driver.cnhCategory}
        </SummaryRow>
        <SummaryRow term="CPF">
          <span className="font-mono">{formatCpf(driver.cpf)}</span>
        </SummaryRow>
        <SummaryRow term="Comissão padrão">{formatPercent(driver.defaultCommissionPercent ?? 0)}</SummaryRow>
        <SummaryRow term="Cadastro">
          <Badge tone={activity.tone}>{activity.label}</Badge>
        </SummaryRow>
      </SummaryList>
    </Card>
  )
}
