import type { RouteObject } from 'react-router'
import { OverviewPage } from '@/features/overview/OverviewPage'
import { NotFoundPage } from '@/features/not-found/NotFoundPage'
import { AppShell } from '@/layout/AppShell'
import { RoutePath } from './routePaths'

// Exportadas separadas do router para que os testes montem a mesma árvore em memória.
export const APP_ROUTES: RouteObject[] = [
  {
    path: RoutePath.overview,
    element: <AppShell />,
    children: [
      { index: true, element: <OverviewPage /> },
      { path: '*', element: <NotFoundPage /> },
    ],
  },
]
