/**
 * Junta classes CSS ignorando valores falsos.
 * @example classNames('h-10', isActive && 'bg-primary') // 'h-10 bg-primary'
 */
export function classNames(...classes: (string | false | null | undefined)[]): string {
  return classes.filter(Boolean).join(' ')
}
