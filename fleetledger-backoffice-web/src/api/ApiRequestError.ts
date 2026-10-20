export interface ApiRequestErrorDetails {
  method: string
  path: string
  status: number
  expected: string
}

/** Falha de chamada à API com método, rota, status recebido e o que se esperava. */
export class ApiRequestError extends Error {
  readonly details: ApiRequestErrorDetails

  constructor(details: ApiRequestErrorDetails) {
    super(`${details.method} ${details.path} respondeu ${details.status}. Esperado: ${details.expected}.`)
    this.name = 'ApiRequestError'
    this.details = details
  }
}
