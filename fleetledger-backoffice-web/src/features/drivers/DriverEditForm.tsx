import { useState, type ReactNode } from 'react'
import { useMutationForm } from '@/api/useMutationForm'
import type { Driver } from './driver'
import { DriverFormCard } from './DriverFormCard'
import { formValuesOf, parseUpdateDriverForm } from './driverForm'
import { useUpdateDriver } from './useDriverMutations'

const SAVED_MESSAGE = 'Motorista atualizado.'

/** Edição de tudo menos o CPF. */
export function DriverEditForm({ driver }: { driver: Driver }): ReactNode {
  const [saved, setSaved] = useState(false)
  const form = useMutationForm({
    initialValues: formValuesOf(driver),
    parse: (values) => {
      setSaved(false)
      return parseUpdateDriverForm(values)
    },
    mutation: useUpdateDriver(driver.id ?? ''),
    onSaved: () => {
      setSaved(true)
    },
  })

  return <DriverFormCard form={form} mode="edit" savedMessage={saved ? SAVED_MESSAGE : null} />
}
