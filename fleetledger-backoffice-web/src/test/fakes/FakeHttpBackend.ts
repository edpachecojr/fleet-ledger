type ResponseFactory = () => Response

interface FakeResponse {
  status: number
  body: unknown
}

/**
 * Servidor HTTP em memória, injetado no `fetch` do cliente real da API.
 * Rotas no formato "MÉTODO /caminho", como chegam na requisição.
 * @example new FakeHttpBackend().respondWithJson('GET /api/health', { status: 200, body: report })
 */
export class FakeHttpBackend {
  readonly receivedRequests: Request[] = []
  private readonly routes = new Map<string, ResponseFactory>()

  respondWithJson(route: string, { status, body }: FakeResponse): this {
    return this.on(route, () => Response.json(body, { status }))
  }

  respondWithText(route: string, { status, body }: FakeResponse): this {
    return this.on(
      route,
      () => new Response(String(body), { status, headers: { 'Content-Type': 'text/plain' } }),
    )
  }

  readonly fetch = (request: Request): Promise<Response> => {
    this.receivedRequests.push(request)
    const route = `${request.method} ${new URL(request.url).pathname}`
    const respond = this.routes.get(route)
    if (!respond)
      throw new Error(
        `FakeHttpBackend sem resposta para "${route}". Registradas: ${[...this.routes.keys()].join(', ')}`,
      )
    return Promise.resolve(respond())
  }

  private on(route: string, respond: ResponseFactory): this {
    this.routes.set(route, respond)
    return this
  }
}
