import {
  EMPTY_VEHICLE_FORM,
  formValuesOf,
  parseCreateVehicleForm,
  parseUpdateVehicleForm,
} from './vehicleForm'

describe('parseCreateVehicleForm', () => {
  it('converts the typed numbers in pt-BR', () => {
    const parsed = parseCreateVehicleForm({
      plate: 'abc-1d23',
      model: 'Volvo FH',
      capacityKg: '27.000',
      initialOdometerKm: '120000',
      maintenanceProvisionRatePerKm: '0,15',
    })

    expect(parsed).toEqual({
      kind: 'parsed',
      request: {
        plate: 'abc-1d23',
        model: 'Volvo FH',
        capacityKg: 27000,
        initialOdometerKm: 120000,
        maintenanceProvisionRatePerKm: 0.15,
      },
    })
  })

  it('flags each unreadable number on its field', () => {
    const parsed = parseCreateVehicleForm({
      ...EMPTY_VEHICLE_FORM,
      capacityKg: 'muito',
      initialOdometerKm: '',
    })

    expect(parsed).toEqual({
      kind: 'invalid',
      fieldErrors: { capacityKg: 'Informe um número.', initialOdometerKm: 'Informe um número.' },
    })
  })
})

describe('parseUpdateVehicleForm', () => {
  it('sends only the editable fields', () => {
    const parsed = parseUpdateVehicleForm({ ...EMPTY_VEHICLE_FORM, model: 'Scania', capacityKg: '30000' })

    expect(parsed).toEqual({
      kind: 'parsed',
      request: { model: 'Scania', capacityKg: 30000, maintenanceProvisionRatePerKm: 0 },
    })
  })
})

describe('formValuesOf', () => {
  it('formats the saved rate with a decimal comma', () => {
    expect(
      formValuesOf({
        plate: 'ABC1D23',
        model: 'Volvo',
        capacityKg: 27000,
        maintenanceProvisionRatePerKm: 0.5,
      }),
    ).toMatchObject({ capacityKg: '27000', maintenanceProvisionRatePerKm: '0,50' })
  })
})
