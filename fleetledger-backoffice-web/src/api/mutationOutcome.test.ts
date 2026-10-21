import { outcomeOf, rejectionOf, VALIDATION_FAILED_MESSAGE } from './mutationOutcome'

describe('rejectionOf', () => {
  it('maps a validation problem to camelCase field errors', () => {
    const rejection = rejectionOf(400, {
      errors: { Plate: ['Placa inválida'], CapacityKg: ['Capacidade deve ser maior que zero'] },
    })

    expect(rejection).toEqual({
      kind: 'rejected',
      message: VALIDATION_FAILED_MESSAGE,
      fieldErrors: { plate: 'Placa inválida', capacityKg: 'Capacidade deve ser maior que zero' },
    })
  })

  it.each([404, 409, 422])('keeps the API message on %s', (status) => {
    expect(
      rejectionOf(status, { data: null, code: status, message: 'Já existe um veículo com a placa ABC1D23.' }),
    ).toEqual({ kind: 'rejected', message: 'Já existe um veículo com a placa ABC1D23.', fieldErrors: {} })
  })

  it('falls back to a generic message when the body has none', () => {
    expect(rejectionOf(409, 'Conflict')?.message).toBe('A operação foi recusada.')
  })

  it('returns null for statuses that are not expected rejections', () => {
    expect(rejectionOf(500, {})).toBeNull()
    expect(rejectionOf(502, 'Bad Gateway')).toBeNull()
  })
})

describe('outcomeOf', () => {
  const description = { method: 'POST', path: '/v1/vehicles', expected: 'ResponseOfVehicle (201)' }

  it('returns the saved value', () => {
    const outcome = outcomeOf(
      { data: { data: { id: '1' } }, response: new Response(null, { status: 201 }) },
      description,
    )

    expect(outcome).toEqual({ kind: 'saved', value: { id: '1' } })
  })

  it('returns the rejection of an expected refusal', () => {
    const outcome = outcomeOf(
      { error: { message: 'Duplicada' }, response: new Response(null, { status: 409 }) },
      description,
    )

    expect(outcome).toMatchObject({ kind: 'rejected', message: 'Duplicada' })
  })

  it('throws with context when the response is outside the contract', () => {
    expect(() =>
      outcomeOf({ error: 'Bad Gateway', response: new Response(null, { status: 502 }) }, description),
    ).toThrow('POST /v1/vehicles respondeu 502')
  })
})
