using FleetLedger.Core.Models;

namespace FleetLedger.Api.Data.Stores.LedgerAccounts;

public interface ILedgerAccountStore
{
    Task<List<LedgerAccountGroup>> ListGroupsWithAccountsAsync(CancellationToken cancellationToken);

    Task<List<LedgerAccount>> ListByEntryContextAsync(ELedgerEntryContext entryContext, CancellationToken cancellationToken);
}
