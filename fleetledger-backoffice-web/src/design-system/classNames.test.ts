import { classNames } from './classNames'

describe('classNames', () => {
  it('joins truthy classes and skips falsy ones', () => {
    expect(classNames('h-10', false, null, undefined, 'bg-primary')).toBe('h-10 bg-primary')
  })
})
