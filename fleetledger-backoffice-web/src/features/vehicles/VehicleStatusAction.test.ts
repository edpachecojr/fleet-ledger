import { vehicleFixture } from '@/test/fakes/vehicles'
import { manualStatusActionOf } from './VehicleStatusAction'

describe('manualStatusActionOf', () => {
  it('offers maintenance for an available vehicle and release for one in maintenance', () => {
    expect(manualStatusActionOf(vehicleFixture())?.target).toBe('InMaintenance')
    expect(manualStatusActionOf(vehicleFixture({ status: 'InMaintenance' }))?.target).toBe('Available')
  })

  it('offers nothing while on trip or inactive', () => {
    expect(manualStatusActionOf(vehicleFixture({ status: 'OnTrip' }))).toBeNull()
    expect(manualStatusActionOf(vehicleFixture({ isActive: false }))).toBeNull()
  })
})
