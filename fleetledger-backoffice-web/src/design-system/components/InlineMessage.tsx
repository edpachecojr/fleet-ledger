import type { ReactNode } from 'react'
import { classNames } from '../classNames'

type InlineMessageTone = 'error' | 'success'

const TONE_CLASSES: Record<InlineMessageTone, string> = {
  error: 'bg-error-soft text-error-text',
  success: 'bg-success-soft text-success-text',
}

/** Retorno de um envio de formulário (recusa da API ou confirmação), anunciado ao leitor de tela. */
export function InlineMessage({
  tone,
  children,
}: {
  tone: InlineMessageTone
  children: ReactNode
}): ReactNode {
  return (
    <p
      role={tone === 'error' ? 'alert' : 'status'}
      className={classNames('rounded-md px-3 py-2 text-sm', TONE_CLASSES[tone])}
    >
      {children}
    </p>
  )
}
