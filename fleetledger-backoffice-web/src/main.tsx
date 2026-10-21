import '@fontsource-variable/outfit'
import '@fontsource-variable/jetbrains-mono'
import './index.css'
import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { createBrowserRouter, RouterProvider } from 'react-router'
import { createApiClient } from '@/api/apiClient'
import { API_BASE_URL } from '@/api/apiConfig'
import { APP_ROUTES } from '@/app/appRoutes'
import { AppProviders } from '@/app/AppProviders'
import { createQueryClient } from '@/app/createQueryClient'
import { AdminSession } from '@/session/AdminSession'

const rootElement = document.getElementById('root')
if (!rootElement) throw new Error('Elemento #root ausente em index.html. Esperado: <div id="root"></div>.')

const session = new AdminSession({ storage: window.sessionStorage, now: () => Date.now() })
const apiClient = createApiClient({ baseUrl: API_BASE_URL, accessTokenSource: session })

createRoot(rootElement).render(
  <StrictMode>
    <AppProviders apiClient={apiClient} queryClient={createQueryClient()} session={session}>
      <RouterProvider router={createBrowserRouter(APP_ROUTES)} />
    </AppProviders>
  </StrictMode>,
)
