import type { ReactNode } from 'react'
import { presentActivity } from '@/api/listQuery'
import { Badge } from '@/design-system/components/Badge'
import { Card } from '@/design-system/components/Card'
import { SummaryList, SummaryRow } from '@/design-system/components/SummaryList'
import { formatCnpj } from '@/design-system/documentFormat'
import { formatPaymentTerm, labelBillingCycle, type Customer } from './customer'

/** Dados de faturamento do cliente: ciclo e prazo em destaque (TASK-006). */
export function CustomerSummaryCard({ customer }: { customer: Customer }): ReactNode {
  const activity = presentActivity(customer.isActive)

  return (
    <Card title="Faturamento">
      <SummaryList>
        <SummaryRow term="Ciclo de faturamento">{labelBillingCycle(customer.billingCycle)}</SummaryRow>
        <SummaryRow term="Prazo de pagamento">{formatPaymentTerm(customer.paymentTermDays)}</SummaryRow>
        <SummaryRow term="CNPJ">
          <span className="font-mono">{formatCnpj(customer.cnpj)}</span>
        </SummaryRow>
        <SummaryRow term="Cadastro">
          <Badge tone={activity.tone}>{activity.label}</Badge>
        </SummaryRow>
      </SummaryList>
    </Card>
  )
}
