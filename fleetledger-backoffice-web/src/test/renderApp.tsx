import { QueryClient } from '@tanstack/react-query'
import { render, type RenderResult } from '@testing-library/react'
import userEvent, { type UserEvent } from '@testing-library/user-event'
import { createMemoryRouter, RouterProvider } from 'react-router'
import { anonymousAccessTokenSource } from '@/api/accessTokenSource'
import { createApiClient, type ApiClient } from '@/api/apiClient'
import { APP_ROUTES } from '@/app/appRoutes'
import { AppProviders } from '@/app/AppProviders'
import type { FakeHttpBackend } from './fakes/FakeHttpBackend'

export const TEST_API_BASE_URL = 'http://backoffice.test/api'

export function createTestApiClient(backend: FakeHttpBackend): ApiClient {
  return createApiClient({
    baseUrl: TEST_API_BASE_URL,
    accessTokenSource: anonymousAccessTokenSource,
    fetch: backend.fetch,
  })
}

/** Monta a aplicação inteira em memória, na rota pedida, falando com o backend falso. */
export function renderApp(backend: FakeHttpBackend, route = '/'): RenderResult & { user: UserEvent } {
  const queryClient = new QueryClient({ defaultOptions: { queries: { retry: false } } })
  const router = createMemoryRouter(APP_ROUTES, { initialEntries: [route] })

  const result = render(
    <AppProviders apiClient={createTestApiClient(backend)} queryClient={queryClient}>
      <RouterProvider router={router} />
    </AppProviders>,
  )
  return { ...result, user: userEvent.setup() }
}
