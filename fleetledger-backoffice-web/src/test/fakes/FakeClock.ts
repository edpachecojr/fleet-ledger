/** Relógio controlado pelo teste: começa num instante fixo e só anda com `advance`. */
export class FakeClock {
  private current: number

  constructor(isoInstant = '2026-10-20T20:00:00Z') {
    this.current = Date.parse(isoInstant)
  }

  readonly now = (): number => this.current

  advance(milliseconds: number): void {
    this.current += milliseconds
  }
}
