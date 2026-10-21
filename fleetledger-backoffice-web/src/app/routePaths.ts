export const RoutePath = {
  overview: '/',
  login: '/login',
  chartOfAccounts: '/plano-de-contas',
  vehicles: '/veiculos',
  newVehicle: '/veiculos/novo',
  vehicleDetail: '/veiculos/:vehicleId',
} as const

/**
 * Rota do detalhe de um veículo.
 * @example vehicleDetailPath('0190…') // '/veiculos/0190…'
 */
export function vehicleDetailPath(vehicleId: string): string {
  return `${RoutePath.vehicles}/${vehicleId}`
}
