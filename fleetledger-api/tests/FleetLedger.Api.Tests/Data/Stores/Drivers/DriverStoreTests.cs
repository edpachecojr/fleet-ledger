using FleetLedger.Api.Data;
using FleetLedger.Api.Data.Stores.Drivers;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Drivers;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Tests.Data.Stores.Drivers;

public class DriverStoreTests
{
    private readonly Guid _organizationId = Guid.CreateVersion7();

    [Fact]
    public void ByIdQuery_FiltersByOrganizationAndId()
    {
        using AppDbContext context = OfflineDbContextFactory.Create();

        string sql = new DriverStore(context).ByIdQuery(_organizationId, Guid.CreateVersion7()).ToQueryString();

        sql.Should().MatchRegex(@"d\.organization_id = @\w+").And.MatchRegex(@"d\.id = @\w+");
    }

    [Fact]
    public void ListQuery_FiltersByOrganizationAndOrdersByName()
        => ListSql(new ListDriversRequest { OrganizationId = _organizationId })
            .Should().MatchRegex(@"WHERE d\.organization_id = @\w+").And.MatchRegex(@"ORDER BY d\.name");

    [Fact]
    public void ListQuery_WithSearch_MatchesNameOrCpf()
        => ListSql(new ListDriversRequest { OrganizationId = _organizationId, Search = "529.98" })
            .Should().Contain("d.name ILIKE").And.Contain("d.cpf LIKE");

    [Fact]
    public void ListQuery_WithExpiredStatus_ComparesBothExpirationsWithToday()
        => ListSql(new ListDriversRequest { OrganizationId = _organizationId, DocumentStatus = EDocumentStatus.Expired })
            .Should().Contain("d.cnh_expires_on <").And.Contain("d.toxicology_exam_expires_on <");

    [Fact]
    public void ListQuery_WhenOnlyActive_FiltersByDeactivation()
        => ListSql(new ListDriversRequest { OrganizationId = _organizationId, IsActive = true })
            .Should().Contain("deactivated_at IS NULL");

    private static string ListSql(ListDriversRequest request)
    {
        using AppDbContext context = OfflineDbContextFactory.Create();
        return new DriverStore(context).ListQuery(request, FakeBusinessCalendar.ReferenceToday).ToQueryString();
    }
}
