import { classNames } from '../classNames'

/**
 * Controle de formulário do card FormControls: 40px, borda `border-input`, hover
 * `text-primary`, foco com borda e anel `primary`, inválido `error`, desabilitado `neutral-soft`.
 * Compartilhado por TextField, SelectField e a busca da toolbar.
 */
export const CONTROL_CLASSES = classNames(
  'h-10 w-full rounded-md border border-border-input bg-transparent px-3 text-sm transition-colors outline-none',
  'placeholder:text-text-disabled hover:border-text-primary',
  'focus:border-primary focus:ring-1 focus:ring-primary',
  'disabled:bg-neutral-soft disabled:text-text-disabled aria-invalid:border-error',
)
