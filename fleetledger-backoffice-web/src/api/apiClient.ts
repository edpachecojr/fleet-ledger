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

function bearerTokenMiddleware(accessTokenSource: AccessTokenSource): Middleware {
  return {
    onRequest({ request }) {
      const accessToken = accessTokenSource.getAccessToken()
      if (accessToken) request.headers.set('Authorization', `Bearer ${accessToken}`)
      return request
    },
  }
}
