using FleetLedger.Api.Data.Seed;
using FleetLedger.Api.Handlers.LedgerAccounts;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Core.Models;
using FleetLedger.Core.Responses;

namespace FleetLedger.Api.Tests.Handlers.LedgerAccounts;

public class LedgerAccountHandlerTests
{
    private readonly LedgerAccountHandler _handler = new(
        new FakeLedgerAccountStore(ChartOfAccountsCatalog.Groups, ChartOfAccountsCatalog.Accounts));

    [Fact]
    public async Task ListChartAsync_ReturnsEveryGroup()
    {
        Response<List<LedgerAccountGroup>> result = await _handler.ListChartAsync(CancellationToken.None);

        result.Code.Should().Be(200);
        result.Data.Should().HaveCount(ChartOfAccountsCatalog.Groups.Count);
    }

    [Fact]
    public async Task ListSelectableAsync_ReturnsOnlyAccountsOfTheContext()
    {
        Response<List<LedgerAccount>> result = await _handler.ListSelectableAsync(
            ELedgerEntryContext.Maintenance,
            CancellationToken.None);

        result.Data!.Select(account => account.Name).Should().Equal("Manutenção realizada");
    }

    [Fact]
    public async Task ListSelectableAsync_NeverReturnsSystemAccounts()
    {
        IEnumerable<ELedgerEntryContext> contexts = Enum.GetValues<ELedgerEntryContext>();

        List<LedgerAccount> selectable = [];
        foreach (ELedgerEntryContext entryContext in contexts)
            selectable.AddRange((await _handler.ListSelectableAsync(entryContext, CancellationToken.None)).Data!);

        selectable.Should().NotContain(account => account.EntryContext == null);
    }
}
