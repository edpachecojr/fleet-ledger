import type { ReactNode } from 'react'
import { useNavigate } from 'react-router'
import { customerDetailPath } from '@/app/routePaths'
import { useMutationForm } from '@/api/useMutationForm'
import { PageHeader } from '@/design-system/components/PageHeader'
import { CustomerFormCard } from './CustomerFormCard'
import { EMPTY_CUSTOMER_FORM, parseCreateCustomerForm } from './customerForm'
import { useCreateCustomer } from './useCustomerMutations'

/** Cadastro de cliente; salvo, abre no detalhe. */
export function NewCustomerPage(): ReactNode {
  const navigate = useNavigate()
  const form = useMutationForm({
    initialValues: EMPTY_CUSTOMER_FORM,
    parse: parseCreateCustomerForm,
    mutation: useCreateCustomer(),
    onSaved: (customer) => {
      void navigate(customerDetailPath(customer.id ?? ''))
    },
  })

  return (
    <>
      <PageHeader
        title="Novo cliente"
        description="Razão social, CNPJ e ciclo de faturamento são obrigatórios."
      />
      <CustomerFormCard form={form} mode="create" />
    </>
  )
}
