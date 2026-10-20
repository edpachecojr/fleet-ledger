import '@fontsource-variable/outfit'
import '@fontsource-variable/jetbrains-mono'
import './index.css'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router'
import { anonymousAccessTokenSource } from '@/api/accessTokenSource'
import { createApiClient } from '@/api/apiClient'
import { API_BASE_URL } from '@/api/apiConfig'
import { APP_ROUTES } from '@/app/appRoutes'
import { AppProviders } from '@/app/AppProviders'
import { createQueryClient } from '@/app/createQueryClient'

const rootElement = document.getElementById('root')
if (!rootElement) throw new Error('Elemento #root ausente em index.html. Esperado: <div id="root"></div>.')

// TASK-002 troca anonymousAccessTokenSource pela sessão do administrador.
const apiClient = createApiClient({ baseUrl: API_BASE_URL, accessTokenSource: anonymousAccessTokenSource })

createRoot(rootElement).render(
  <StrictMode>
    <AppProviders apiClient={apiClient} queryClient={createQueryClient()}>
      <RouterProvider router={createBrowserRouter(APP_ROUTES)} />
    </AppProviders>
  </StrictMode>,
)
