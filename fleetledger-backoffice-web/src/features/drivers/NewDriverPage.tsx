import type { ReactNode } from 'react'
import { useNavigate } from 'react-router'
import { driverDetailPath } from '@/app/routePaths'
import { useMutationForm } from '@/api/useMutationForm'
import { PageHeader } from '@/design-system/components/PageHeader'
import { DriverFormCard } from './DriverFormCard'
import { EMPTY_DRIVER_FORM, parseCreateDriverForm } from './driverForm'
import { useCreateDriver } from './useDriverMutations'

/** Cadastro de motorista; salvo, abre no detalhe. */
export function NewDriverPage(): ReactNode {
  const navigate = useNavigate()
  const form = useMutationForm({
    initialValues: EMPTY_DRIVER_FORM,
    parse: parseCreateDriverForm,
    mutation: useCreateDriver(),
    onSaved: (driver) => {
      void navigate(driverDetailPath(driver.id ?? ''))
    },
  })

  return (
    <>
      <PageHeader
        title="Novo motorista"
        description="Documento vencido só gera aviso na abertura de viagem, sem bloqueio."
      />
      <DriverFormCard form={form} mode="create" />
    </>
  )
}
