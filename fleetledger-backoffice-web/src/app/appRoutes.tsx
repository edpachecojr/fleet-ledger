import type { RouteObject } from 'react-router'
import { ChartOfAccountsPage } from '@/features/chart-of-accounts/ChartOfAccountsPage'
import { LoginPage } from '@/features/login/LoginPage'
import { OverviewPage } from '@/features/overview/OverviewPage'
import { NotFoundPage } from '@/features/not-found/NotFoundPage'
import { AppShell } from '@/layout/AppShell'
import { RequireSession } from '@/session/RequireSession'
import { RoutePath } from './routePaths'

// Exportadas separadas do router para que os testes montem a mesma árvore em memória.
export const APP_ROUTES: RouteObject[] = [
  { path: RoutePath.login, element: <LoginPage /> },
  {
    path: RoutePath.overview,
    element: (
      <RequireSession>
        <AppShell />
      </RequireSession>
    ),
    children: [
      { index: true, element: <OverviewPage /> },
      { path: RoutePath.chartOfAccounts, element: <ChartOfAccountsPage /> },
      { path: '*', element: <NotFoundPage /> },
    ],
  },
]
