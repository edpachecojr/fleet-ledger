/**
 * De onde o cliente HTTP lê o access token (JWT) a cada requisição.
 * A sessão do administrador (TASK-002) implementa esta interface.
 */
export interface AccessTokenSource {
  getAccessToken: () => string | null
}

export const anonymousAccessTokenSource: AccessTokenSource = {
  getAccessToken: () => null,
}
