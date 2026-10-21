using FleetLedger.Api.Handlers.Drivers;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Tests.Handlers.Drivers;

public class DriverDocumentStatusRuleTests
{
    private static readonly DateOnly Today = FakeBusinessCalendar.ReferenceToday;

    [Theory]
    [InlineData(-1, EDocumentStatus.Expired)]
    [InlineData(0, EDocumentStatus.ExpiringSoon)]
    [InlineData(30, EDocumentStatus.ExpiringSoon)]
    [InlineData(31, EDocumentStatus.Regular)]
    public void Evaluate_ByCnhExpiration_UsesTheThirtyDayWindowInclusive(int daysFromToday, EDocumentStatus expected)
        => DriverDocumentStatusRule.Evaluate(Driver(cnhInDays: daysFromToday), Today).Should().Be(expected);

    [Fact]
    public void Evaluate_TakesTheWorstOfTheTwoDocuments()
        => DriverDocumentStatusRule.Evaluate(Driver(cnhInDays: 400, toxicologyInDays: -2), Today)
            .Should().Be(EDocumentStatus.Expired);

    [Fact]
    public void Evaluate_WhenToxicologyIsBlank_ConsidersOnlyTheCnh()
        => DriverDocumentStatusRule.Evaluate(Driver(cnhInDays: 400, toxicologyInDays: null), Today)
            .Should().Be(EDocumentStatus.Regular);

    [Theory]
    [InlineData(-1, null, EDocumentStatus.Expired)]
    [InlineData(400, -1, EDocumentStatus.Expired)]
    [InlineData(10, null, EDocumentStatus.ExpiringSoon)]
    [InlineData(400, 30, EDocumentStatus.ExpiringSoon)]
    [InlineData(31, null, EDocumentStatus.Regular)]
    [InlineData(400, 31, EDocumentStatus.Regular)]
    public void HasStatus_MatchesEvaluateForEveryStatus(int cnhInDays, int? toxicologyInDays, EDocumentStatus status)
    {
        Driver driver = Driver(cnhInDays, toxicologyInDays);

        IEnumerable<EDocumentStatus> matching = Enum.GetValues<EDocumentStatus>()
            .Where(candidate => DriverDocumentStatusRule.HasStatus(candidate, Today).Compile()(driver));

        matching.Should().Equal(status);
        DriverDocumentStatusRule.Evaluate(driver, Today).Should().Be(status);
    }

    private static Driver Driver(int cnhInDays, int? toxicologyInDays = null)
        => DriverFactory.Create(
            Guid.CreateVersion7(),
            cnhExpiresOn: Today.AddDays(cnhInDays),
            toxicologyExamExpiresOn: toxicologyInDays is int days ? Today.AddDays(days) : null);
}
