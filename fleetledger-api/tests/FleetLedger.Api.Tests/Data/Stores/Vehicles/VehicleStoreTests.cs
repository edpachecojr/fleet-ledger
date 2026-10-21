using FleetLedger.Api.Data;
using FleetLedger.Api.Data.Stores.Vehicles;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Requests.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Tests.Data.Stores.Vehicles;

public class VehicleStoreTests
{
    private readonly Guid _organizationId = Guid.CreateVersion7();

    [Fact]
    public void ByIdQuery_FiltersByOrganizationAndId()
    {
        using AppDbContext context = OfflineDbContextFactory.Create();

        string sql = new VehicleStore(context).ByIdQuery(_organizationId, Guid.CreateVersion7()).ToQueryString();

        sql.Should().MatchRegex(@"v\.organization_id = @\w+").And.MatchRegex(@"v\.id = @\w+");
    }

    [Fact]
    public void ListQuery_WithoutFilters_FiltersByOrganizationAndOrdersByPlate()
    {
        string sql = ListSql(new ListVehiclesRequest { OrganizationId = _organizationId });

        sql.Should().MatchRegex(@"WHERE v\.organization_id = @\w+").And.MatchRegex(@"ORDER BY v\.plate");
    }

    [Fact]
    public void ListQuery_WithSearch_MatchesPlateOrModelCaseInsensitively()
    {
        string sql = ListSql(new ListVehiclesRequest { OrganizationId = _organizationId, Search = "abc-1" });

        sql.Should().Contain("v.plate LIKE").And.Contain("v.model ILIKE");
    }

    [Fact]
    public void ListQuery_WhenAllocatable_RequiresActiveAndAvailable()
    {
        string sql = ListSql(new ListVehiclesRequest { OrganizationId = _organizationId, Allocatable = true });

        sql.Should().Contain("v.deactivated_at IS NULL").And.Contain("v.status = 1");
    }

    [Fact]
    public void ListQuery_WhenOnlyInactive_FiltersByDeactivation()
    {
        string sql = ListSql(new ListVehiclesRequest { OrganizationId = _organizationId, IsActive = false });

        sql.Should().Contain("deactivated_at IS NULL");
    }

    private static string ListSql(ListVehiclesRequest request)
    {
        using AppDbContext context = OfflineDbContextFactory.Create();
        return new VehicleStore(context).ListQuery(request).ToQueryString();
    }
}
