import type { Vehicle, VehiclePage } from '@/features/vehicles/vehicle'

export const VEHICLE_ID = '01a15f29-51a6-7099-9bb8-959308633a99'

export function vehicleFixture(overrides: Partial<Vehicle> = {}): Vehicle {
  return {
    id: VEHICLE_ID,
    plate: 'ABC1D23',
    model: 'Volvo FH 540',
    capacityKg: 27000,
    initialOdometerKm: 100000,
    currentOdometerKm: 125000,
    maintenanceProvisionRatePerKm: 0.15,
    status: 'Available',
    isActive: true,
    deactivatedAt: null,
    createdAt: '2026-10-21T12:00:00Z',
    updatedAt: '2026-10-21T12:00:00Z',
    ...overrides,
  }
}

export function vehiclePageBody(vehicles: Vehicle[]): VehiclePage {
  return {
    data: vehicles,
    totalCount: vehicles.length,
    currentPage: 1,
    pageSize: 25,
    totalPages: 1,
    code: 200,
  }
}

export function vehicleBody(vehicle: Vehicle, code = 200): { data: Vehicle; code: number; message: null } {
  return { data: vehicle, code, message: null }
}
