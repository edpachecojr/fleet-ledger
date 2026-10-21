import { useState, type ReactNode } from 'react'
import { useMutationForm } from '@/api/useMutationForm'
import type { Customer } from './customer'
import { CustomerFormCard } from './CustomerFormCard'
import { formValuesOf, parseUpdateCustomerForm } from './customerForm'
import { useUpdateCustomer } from './useCustomerMutations'

const SAVED_MESSAGE = 'Cliente atualizado.'

/** Edição de tudo menos o CNPJ. */
export function CustomerEditForm({ customer }: { customer: Customer }): ReactNode {
  const [saved, setSaved] = useState(false)
  const form = useMutationForm({
    initialValues: formValuesOf(customer),
    parse: (values) => {
      setSaved(false)
      return parseUpdateCustomerForm(values)
    },
    mutation: useUpdateCustomer(customer.id ?? ''),
    onSaved: () => {
      setSaved(true)
    },
  })

  return <CustomerFormCard form={form} mode="edit" savedMessage={saved ? SAVED_MESSAGE : null} />
}
