// O jsdom não implementa matchMedia; o tema depende dele para o modo "sistema".
export class FakeMatchMedia {
  private readonly prefersDark: boolean

  constructor(prefersDark: boolean) {
    this.prefersDark = prefersDark
  }

  install(): void {
    window.matchMedia = (query: string) => this.listFor(query)
  }

  // Só o que o código usa; a conversão evita implementar a API legada (addListener).
  private listFor(query: string): MediaQueryList {
    const list: Pick<MediaQueryList, 'matches' | 'media' | 'addEventListener' | 'removeEventListener'> = {
      matches: query.includes('dark') && this.prefersDark,
      media: query,
      addEventListener: () => undefined,
      removeEventListener: () => undefined,
    }
    return list as MediaQueryList
  }
}
