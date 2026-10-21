using FleetLedger.Api.Data;
using FleetLedger.Api.Data.Stores.Customers;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Requests.Customers;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Tests.Data.Stores.Customers;

public class CustomerStoreTests
{
    private readonly Guid _organizationId = Guid.CreateVersion7();

    [Fact]
    public void ByIdQuery_FiltersByOrganizationAndId()
    {
        using AppDbContext context = OfflineDbContextFactory.Create();

        string sql = new CustomerStore(context).ByIdQuery(_organizationId, Guid.CreateVersion7()).ToQueryString();

        sql.Should().MatchRegex(@"c\.organization_id = @\w+").And.MatchRegex(@"c\.id = @\w+");
    }

    [Fact]
    public void ListQuery_FiltersByOrganizationAndOrdersByCorporateName()
    {
        string sql = ListSql(new ListCustomersRequest { OrganizationId = _organizationId });

        sql.Should().MatchRegex(@"WHERE c\.organization_id = @\w+").And.MatchRegex(@"ORDER BY c\.corporate_name");
    }

    [Fact]
    public void ListQuery_WithSearch_MatchesNamesAndCnpj()
    {
        string sql = ListSql(new ListCustomersRequest { OrganizationId = _organizationId, Search = "11.222" });

        sql.Should().Contain("c.corporate_name ILIKE").And.Contain("c.trade_name ILIKE").And.Contain("c.cnpj LIKE");
    }

    [Fact]
    public void ListQuery_WhenOnlyActive_FiltersByDeactivation()
        => ListSql(new ListCustomersRequest { OrganizationId = _organizationId, IsActive = true })
            .Should().Contain("deactivated_at IS NULL");

    private static string ListSql(ListCustomersRequest request)
    {
        using AppDbContext context = OfflineDbContextFactory.Create();
        return new CustomerStore(context).ListQuery(request).ToQueryString();
    }
}
