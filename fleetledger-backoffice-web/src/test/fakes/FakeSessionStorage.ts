import type { SessionStorageLike } from '@/session/storedSession'

/** sessionStorage em memória, isolado por teste. */
export class FakeSessionStorage implements SessionStorageLike {
  private readonly entries = new Map<string, string>()

  getItem(key: string): string | null {
    return this.entries.get(key) ?? null
  }

  setItem(key: string, value: string): void {
    this.entries.set(key, value)
  }

  removeItem(key: string): void {
    this.entries.delete(key)
  }
}
