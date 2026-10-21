import { screen, within } from '@testing-library/react'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { LEDGER_ACCOUNT_GROUPS } from '@/test/fakes/ledgerAccountGroups'
import { renderApp } from '@/test/renderApp'

const CHART_ROUTE = 'GET /api/v1/ledger-accounts'

describe('ChartOfAccountsPage', () => {
  it('lists each group with its accounts in hierarchy', async () => {
    renderApp(
      new FakeHttpBackend().respondWithJson(CHART_ROUTE, {
        status: 200,
        body: { data: LEDGER_ACCOUNT_GROUPS, message: null, code: 200 },
      }),
      '/plano-de-contas',
    )

    const tripGroup = await screen.findByRole('table', { name: '3 · Despesas variáveis de viagem' })
    expect(within(tripGroup).getByRole('row', { name: /3\.01 Combustível Viagem/ })).toBeInTheDocument()
    expect(screen.getByRole('table', { name: '7 · Movimentações não-resultado' })).toBeInTheDocument()
  })

  it('marks system accounts instead of a manual entry context', async () => {
    renderApp(
      new FakeHttpBackend().respondWithJson(CHART_ROUTE, {
        status: 200,
        body: { data: LEDGER_ACCOUNT_GROUPS, message: null, code: 200 },
      }),
      '/plano-de-contas',
    )

    const advanceRow = await screen.findByRole('row', { name: /Adiantamento a motorista/ })
    expect(within(advanceRow).getByText('Sistêmica')).toBeInTheDocument()
  })

  it('offers no action to change the chart', async () => {
    renderApp(
      new FakeHttpBackend().respondWithJson(CHART_ROUTE, {
        status: 200,
        body: { data: LEDGER_ACCOUNT_GROUPS, message: null, code: 200 },
      }),
      '/plano-de-contas',
    )

    const main = await screen.findByRole('main')
    await screen.findByRole('table', { name: '3 · Despesas variáveis de viagem' })
    expect(within(main).queryAllByRole('button')).toHaveLength(0)
  })

  it('tells the user when the chart cannot be loaded', async () => {
    renderApp(
      new FakeHttpBackend().respondWithText(CHART_ROUTE, { status: 502, body: 'Bad Gateway' }),
      '/plano-de-contas',
    )

    expect(await screen.findByText('Plano de contas indisponível')).toBeInTheDocument()
  })
})
