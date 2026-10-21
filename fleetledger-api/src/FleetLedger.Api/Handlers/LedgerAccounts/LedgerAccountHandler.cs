using FleetLedger.Api.Data.Stores.LedgerAccounts;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Responses;

namespace FleetLedger.Api.Handlers.LedgerAccounts;

// Somente leitura: o Plano de Contas não tem operação de escrita (TASK-003).
public sealed class LedgerAccountHandler(ILedgerAccountStore store) : ILedgerAccountHandler
{
    public async Task<Response<List<LedgerAccountGroup>>> ListChartAsync(CancellationToken cancellationToken)
        => new(await store.ListGroupsWithAccountsAsync(cancellationToken));

    public async Task<Response<List<LedgerAccount>>> ListSelectableAsync(
        ELedgerEntryContext entryContext,
        CancellationToken cancellationToken)
        => new(await store.ListByEntryContextAsync(entryContext, cancellationToken));
}
