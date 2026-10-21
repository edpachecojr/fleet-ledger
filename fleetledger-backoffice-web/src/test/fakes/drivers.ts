import type { Driver, DriverPage } from '@/features/drivers/driver'

export const DRIVER_ID = '01a15f77-0000-7000-8000-000000000001'

export function driverFixture(overrides: Partial<Driver> = {}): Driver {
  return {
    id: DRIVER_ID,
    name: 'João da Silva',
    cpf: '52998224725',
    phone: '11987654321',
    cnhNumber: '12345678901',
    cnhCategory: 'E',
    cnhExpiresOn: '2028-05-10',
    toxicologyExamExpiresOn: '2026-11-10',
    defaultCommissionPercent: 12.5,
    bankDetails: 'PIX: 11987654321',
    documentStatus: 'ExpiringSoon',
    isActive: true,
    deactivatedAt: null,
    createdAt: '2026-10-21T12:00:00Z',
    updatedAt: '2026-10-21T12:00:00Z',
    ...overrides,
  }
}

export function driverPageBody(drivers: Driver[]): DriverPage {
  return { data: drivers, totalCount: drivers.length, currentPage: 1, pageSize: 25, totalPages: 1, code: 200 }
}

export function driverBody(driver: Driver, code = 200): { data: Driver; code: number; message: null } {
  return { data: driver, code, message: null }
}
