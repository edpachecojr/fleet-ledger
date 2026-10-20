import '@testing-library/jest-dom/vitest'
import { FakeMatchMedia } from './fakes/FakeMatchMedia'

beforeEach(() => {
  new FakeMatchMedia(false).install()
  localStorage.clear()
})
