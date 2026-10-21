import type { ReactNode } from 'react'
import { Link } from 'react-router'
import { driverDetailPath } from '@/app/routePaths'
import { presentActivity } from '@/api/listQuery'
import { Badge, type BadgePresentation } from '@/design-system/components/Badge'
import type { DataTableColumn } from '@/design-system/components/DataTable'
import { formatDateOnly } from '@/design-system/dateFormat'
import { formatCpf, formatPhone } from '@/design-system/documentFormat'
import { formatPercent } from '@/design-system/numberFormat'
import { presentDocumentStatus, type Driver } from './driver'

export const DRIVER_TABLE_COLUMNS: readonly DataTableColumn<Driver>[] = [
  { key: 'name', header: 'Motorista', render: (driver) => <DriverNameCell driver={driver} /> },
  {
    key: 'phone',
    header: 'Telefone',
    render: (driver) => <span className="tabular-nums">{formatPhone(driver.phone)}</span>,
  },
  { key: 'cnh', header: 'CNH', render: (driver) => <CnhCell driver={driver} /> },
  {
    key: 'toxicology',
    header: 'Toxicológico',
    render: (driver) => (
      <span className="tabular-nums">{formatDateOnly(driver.toxicologyExamExpiresOn)}</span>
    ),
  },
  {
    key: 'commission',
    header: 'Comissão',
    align: 'end',
    render: (driver) => formatPercent(driver.defaultCommissionPercent ?? 0),
  },
  {
    key: 'documents',
    header: 'Documentos',
    render: (driver) => <PresentedBadge {...presentDocumentStatus(driver.documentStatus)} />,
  },
  {
    key: 'activity',
    header: 'Situação',
    render: (driver) => <PresentedBadge {...presentActivity(driver.isActive)} />,
  },
]

// CPF embaixo do nome, em mono secundário, como o SKU no card DataTable.
function DriverNameCell({ driver }: { driver: Driver }): ReactNode {
  return (
    <div className="grid">
      <Link to={driverDetailPath(driver.id ?? '')} className="font-semibold text-primary hover:underline">
        {driver.name}
      </Link>
      <span className="font-mono text-xs text-text-secondary">{formatCpf(driver.cpf)}</span>
    </div>
  )
}

function CnhCell({ driver }: { driver: Driver }): ReactNode {
  return (
    <div className="grid text-[13px]">
      <span>Categoria {driver.cnhCategory}</span>
      <span className="text-text-secondary tabular-nums">Vence {formatDateOnly(driver.cnhExpiresOn)}</span>
    </div>
  )
}

function PresentedBadge({ label, tone }: BadgePresentation): ReactNode {
  return <Badge tone={tone}>{label}</Badge>
}
