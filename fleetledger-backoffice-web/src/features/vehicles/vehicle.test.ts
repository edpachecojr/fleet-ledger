import {
  formatProvisionRate,
  isVehicleStatusFilter,
  presentVehicleActivity,
  presentVehicleStatus,
} from './vehicle'

describe('presentVehicleStatus', () => {
  it.each([
    ['Available', 'Disponível', 'success'],
    ['OnTrip', 'Em viagem', 'info'],
    ['InMaintenance', 'Em manutenção', 'warning'],
  ] as const)('maps %s to "%s" (%s)', (status, label, tone) => {
    expect(presentVehicleStatus(status)).toEqual({ label, tone })
  })
})

describe('presentVehicleActivity', () => {
  it('tells active and inactive vehicles apart', () => {
    expect(presentVehicleActivity({ isActive: true }).label).toBe('Ativo')
    expect(presentVehicleActivity({ isActive: false })).toEqual({ label: 'Inativo', tone: 'neutral' })
  })
})

describe('formatProvisionRate', () => {
  it('formats reais per km', () => {
    expect(formatProvisionRate(0.15).replace(/\s/g, ' ')).toBe('R$ 0,15/km')
  })
})

describe('isVehicleStatusFilter', () => {
  it('accepts every status and "all", nothing else', () => {
    expect(['all', 'Available', 'OnTrip', 'InMaintenance'].every(isVehicleStatusFilter)).toBe(true)
    expect(isVehicleStatusFilter('Parked')).toBe(false)
  })
})
