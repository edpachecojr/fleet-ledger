import { ApiRequestError } from '@/api/ApiRequestError'
import { FakeHttpBackend } from '@/test/fakes/FakeHttpBackend'
import { LEDGER_ACCOUNT_GROUPS } from '@/test/fakes/ledgerAccountGroups'
import { createTestApiClient } from '@/test/renderApp'
import { fetchChartOfAccounts } from './fetchChartOfAccounts'

describe('fetchChartOfAccounts', () => {
  it('returns the groups of the chart', async () => {
    const backend = new FakeHttpBackend().respondWithJson('GET /api/v1/ledger-accounts', {
      status: 200,
      body: { data: LEDGER_ACCOUNT_GROUPS, message: null, code: 200 },
    })

    const groups = await fetchChartOfAccounts(createTestApiClient(backend))

    expect(groups.map((group) => group.code)).toEqual(['3', '7'])
  })

  it('throws with context when the API does not answer the chart', async () => {
    const backend = new FakeHttpBackend().respondWithText('GET /api/v1/ledger-accounts', {
      status: 502,
      body: 'Bad Gateway',
    })

    const request = fetchChartOfAccounts(createTestApiClient(backend))

    await expect(request).rejects.toThrow(ApiRequestError)
    await expect(request).rejects.toThrow('GET /v1/ledger-accounts respondeu 502')
  })
})
