import createClient, { type Client, type Middleware } from 'openapi-fetch'
import type { AccessTokenSource } from './accessTokenSource'
import type { paths } from './generated/schema'

export type ApiClient = Client<paths>

export interface ApiClientOptions {
  baseUrl: string
  accessTokenSource: AccessTokenSource
  fetch?: (request: Request) => Promise<Response>
}

/**
 * Cliente tipado pelo contrato OpenAPI da API (src/api/generated/schema.d.ts).
 * @example const client = createApiClient({ baseUrl: '/api', accessTokenSource: anonymousAccessTokenSource })
 */
export function createApiClient(options: ApiClientOptions): ApiClient {
  const client = createClient<paths>({
    baseUrl: options.baseUrl,
    ...(options.fetch ? { fetch: options.fetch } : {}),
  })
  client.use(bearerTokenMiddleware(options.accessTokenSource))
  return client
}

const AUTHORIZATION_HEADER = 'Authorization'
const UNAUTHORIZED_STATUS = 401

function bearerTokenMiddleware(accessTokenSource: AccessTokenSource): Middleware {
  return {
    onRequest({ request }) {
      const accessToken = accessTokenSource.getAccessToken()
      if (accessToken) request.headers.set(AUTHORIZATION_HEADER, `Bearer ${accessToken}`)
      return request
    },
    // Só 401 de requisição que levou token encerra a sessão: o 401 do login
    // (credencial errada) é resposta esperada, não sessão vencida.
    onResponse({ request, response }) {
      if (response.status === UNAUTHORIZED_STATUS && request.headers.has(AUTHORIZATION_HEADER))
        accessTokenSource.discardAccessToken()
      return response
    },
  }
}
