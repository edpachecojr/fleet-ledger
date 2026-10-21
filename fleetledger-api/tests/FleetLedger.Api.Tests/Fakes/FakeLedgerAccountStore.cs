using FleetLedger.Api.Data.Stores.LedgerAccounts;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Tests.Fakes;

public sealed class FakeLedgerAccountStore(IReadOnlyList<LedgerAccountGroup> groups, IReadOnlyList<LedgerAccount> accounts)
    : ILedgerAccountStore
{
    public Task<List<LedgerAccountGroup>> ListGroupsWithAccountsAsync(CancellationToken cancellationToken)
        => Task.FromResult(groups.ToList());

    public Task<List<LedgerAccount>> ListByEntryContextAsync(
        ELedgerEntryContext entryContext,
        CancellationToken cancellationToken)
        => Task.FromResult(accounts.Where(account => account.EntryContext == entryContext).ToList());
}
