import { RoutePath } from '@/app/routePaths'

/**
 * Para onde ir depois do login: a rota que pediu login, se for interna; senão, a inicial.
 * Só caminho relativo ("/..."), para o estado de navegação não virar redirecionamento externo.
 * @example redirectTargetAfterLogin({ from: '/veiculos' }) // '/veiculos'
 */
export function redirectTargetAfterLogin(navigationState: unknown): string {
  if (typeof navigationState !== 'object' || navigationState === null || !('from' in navigationState))
    return RoutePath.overview
  const { from } = navigationState
  return isInternalPath(from) ? from : RoutePath.overview
}

function isInternalPath(path: unknown): path is string {
  return (
    typeof path === 'string' && path.startsWith('/') && !path.startsWith('//') && path !== RoutePath.login
  )
}
