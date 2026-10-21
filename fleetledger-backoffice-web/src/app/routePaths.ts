export const RoutePath = {
  overview: '/',
  login: '/login',
  chartOfAccounts: '/plano-de-contas',
  vehicles: '/veiculos',
  newVehicle: '/veiculos/novo',
  vehicleDetail: '/veiculos/:vehicleId',
  customers: '/clientes',
  newCustomer: '/clientes/novo',
  customerDetail: '/clientes/:customerId',
} as const

/**
 * Rota do detalhe de um veículo.
 * @example vehicleDetailPath('0190…') // '/veiculos/0190…'
 */
export function vehicleDetailPath(vehicleId: string): string {
  return `${RoutePath.vehicles}/${vehicleId}`
}

/** Rota do detalhe de um cliente. */
export function customerDetailPath(customerId: string): string {
  return `${RoutePath.customers}/${customerId}`
}
