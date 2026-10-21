import type { ReactNode } from 'react'
import { Link } from 'react-router'
import { customerDetailPath } from '@/app/routePaths'
import { presentActivity } from '@/api/listQuery'
import { Badge } from '@/design-system/components/Badge'
import type { DataTableColumn } from '@/design-system/components/DataTable'
import { formatCnpj, formatPhone } from '@/design-system/documentFormat'
import { formatPaymentTerm, labelBillingCycle, type Customer } from './customer'

export const CUSTOMER_TABLE_COLUMNS: readonly DataTableColumn<Customer>[] = [
  { key: 'name', header: 'Cliente', render: (customer) => <CustomerNameCell customer={customer} /> },
  {
    key: 'cnpj',
    header: 'CNPJ',
    render: (customer) => <span className="font-mono text-[13px]">{formatCnpj(customer.cnpj)}</span>,
  },
  {
    key: 'billingCycle',
    header: 'Ciclo de faturamento',
    render: (customer) => labelBillingCycle(customer.billingCycle),
  },
  {
    key: 'paymentTerm',
    header: 'Prazo',
    align: 'end',
    render: (customer) => formatPaymentTerm(customer.paymentTermDays),
  },
  { key: 'contact', header: 'Contato', render: (customer) => <ContactCell customer={customer} /> },
  {
    key: 'activity',
    header: 'Situação',
    render: (customer) => <ActivityBadge isActive={customer.isActive} />,
  },
]

// Nome fantasia embaixo, em texto secundário, como o SKU no card DataTable.
function CustomerNameCell({ customer }: { customer: Customer }): ReactNode {
  return (
    <div className="grid">
      <Link to={customerDetailPath(customer.id ?? '')} className="font-semibold text-primary hover:underline">
        {customer.corporateName}
      </Link>
      {customer.tradeName && <span className="text-xs text-text-secondary">{customer.tradeName}</span>}
    </div>
  )
}

function ContactCell({ customer }: { customer: Customer }): ReactNode {
  if (!customer.phone && !customer.email) return <span className="text-text-disabled">—</span>
  return (
    <div className="grid text-[13px]">
      {customer.phone && <span className="tabular-nums">{formatPhone(customer.phone)}</span>}
      {customer.email && <span className="text-text-secondary">{customer.email}</span>}
    </div>
  )
}

function ActivityBadge({ isActive }: { isActive: boolean | undefined }): ReactNode {
  const activity = presentActivity(isActive)
  return <Badge tone={activity.tone}>{activity.label}</Badge>
}
