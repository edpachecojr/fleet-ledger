/**
 * De onde o cliente HTTP lê o access token (JWT) a cada requisição.
 * A sessão do administrador (session/AdminSession) implementa esta interface.
 */
export interface AccessTokenSource {
  getAccessToken: () => string | null
  /** Chamado quando a API recusa o token enviado (401): expirou ou foi invalidado. */
  discardAccessToken: () => void
}

export const anonymousAccessTokenSource: AccessTokenSource = {
  getAccessToken: () => null,
  discardAccessToken: () => undefined,
}
