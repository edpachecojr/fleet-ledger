import type { RouteObject } from 'react-router'
import { ChartOfAccountsPage } from '@/features/chart-of-accounts/ChartOfAccountsPage'
import { CustomerDetailPage } from '@/features/customers/CustomerDetailPage'
import { CustomersPage } from '@/features/customers/CustomersPage'
import { NewCustomerPage } from '@/features/customers/NewCustomerPage'
import { LoginPage } from '@/features/login/LoginPage'
import { NewVehiclePage } from '@/features/vehicles/NewVehiclePage'
import { VehicleDetailPage } from '@/features/vehicles/VehicleDetailPage'
import { VehiclesPage } from '@/features/vehicles/VehiclesPage'
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
      { path: RoutePath.vehicles, element: <VehiclesPage /> },
      { path: RoutePath.newVehicle, element: <NewVehiclePage /> },
      { path: RoutePath.vehicleDetail, element: <VehicleDetailPage /> },
      { path: RoutePath.customers, element: <CustomersPage /> },
      { path: RoutePath.newCustomer, element: <NewCustomerPage /> },
      { path: RoutePath.customerDetail, element: <CustomerDetailPage /> },
      { path: '*', element: <NotFoundPage /> },
    ],
  },
]
