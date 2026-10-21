using FleetLedger.Api.Data;
using FleetLedger.Api.Data.Stores.LedgerAccounts;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Tests.Data.Stores.LedgerAccounts;

public class LedgerAccountStoreTests
{
    [Fact]
    public void ByEntryContextQuery_FiltersByContextAndOrdersByCode()
    {
        using AppDbContext context = OfflineDbContextFactory.Create();

        string sql = new LedgerAccountStore(context).ByEntryContextQuery(ELedgerEntryContext.Trip).ToQueryString();

        sql.Should().MatchRegex(@"WHERE l\.entry_context = @\w+").And.MatchRegex(@"ORDER BY l\.code");
    }

    [Fact]
    public void GroupsWithAccountsQuery_JoinsAccountsAndOrdersGroupsByCode()
    {
        using AppDbContext context = OfflineDbContextFactory.Create();

        string sql = new LedgerAccountStore(context).GroupsWithAccountsQuery().ToQueryString();

        sql.Should().Contain("ledger_accounts").And.MatchRegex(@"ORDER BY l\.code");
    }
}
