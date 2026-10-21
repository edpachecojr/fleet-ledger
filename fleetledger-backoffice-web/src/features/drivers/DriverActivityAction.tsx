import type { ReactNode } from 'react'
import { ActivityToggle } from '@/design-system/components/ActivityToggle'
import type { Driver } from './driver'
import { useChangeDriverActivity } from './useDriverMutations'

/** Inativar (com confirmação) ou reativar o motorista. */
export function DriverActivityAction({ driver }: { driver: Driver }): ReactNode {
  const activity = useChangeDriverActivity(driver.id ?? '')

  return (
    <ActivityToggle
      isActive={driver.isActive !== false}
      confirmTitle="Inativar motorista?"
      consequence={
        <>
          O motorista <b className="text-text-primary">{driver.name}</b> deixa de aparecer na abertura de nova
          viagem. Você pode reativá-lo depois.
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
