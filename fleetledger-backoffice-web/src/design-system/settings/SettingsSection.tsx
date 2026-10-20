import type { ReactNode } from 'react'

export function SettingsSection({ label, children }: { label: string; children: ReactNode }): ReactNode {
  return (
    <section>
      <h3 className="mb-3 text-xs font-semibold tracking-[0.06em] text-text-secondary uppercase">{label}</h3>
      {children}
    </section>
  )
}
