import { redirectTargetAfterLogin } from './loginRedirect'

describe('redirectTargetAfterLogin', () => {
  it('returns to the screen that asked for login', () => {
    expect(redirectTargetAfterLogin({ from: '/veiculos' })).toBe('/veiculos')
  })

  it.each([
    ['no navigation state', null],
    ['state without origin', {}],
    ['external URL', { from: 'https://evil.example' }],
    ['protocol-relative URL', { from: '//evil.example' }],
    ['the login page itself', { from: '/login' }],
  ])('goes to the overview with %s', (_, state) => {
    expect(redirectTargetAfterLogin(state)).toBe('/')
  })
})
