import type { ReactNode } from 'react'
import { ActivityToggle } from '@/design-system/components/ActivityToggle'
import type { Customer } from './customer'
import { useChangeCustomerActivity } from './useCustomerMutations'

/** Inativar (com confirmação) ou reativar o cliente. */
export function CustomerActivityAction({ customer }: { customer: Customer }): ReactNode {
  const activity = useChangeCustomerActivity(customer.id ?? '')

  return (
    <ActivityToggle
      isActive={customer.isActive !== false}
      confirmTitle="Inativar cliente?"
      consequence={
        <>
          O cliente <b className="text-text-primary">{customer.corporateName}</b> deixa de aparecer na
          abertura de nova viagem. Você pode reativá-lo depois.
        </>
      }
      pending={activity.isPending}
      rejection={activity.data?.kind === 'rejected' ? activity.data.message : null}
      onChange={(deactivate, done) => {
        activity.mutate(deactivate, { onSettled: done })
      }}
    />
  )
}
