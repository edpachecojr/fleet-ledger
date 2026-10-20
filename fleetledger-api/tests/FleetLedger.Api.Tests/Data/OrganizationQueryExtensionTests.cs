using FleetLedger.Api.Data;
using FleetLedger.Api.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Tests.Data;

public class OrganizationQueryExtensionTests
{
    [Fact]
    public void ForOrganization_WhenOrganizationIsSet_FiltersByOrganizationColumn()
    {
        using AppDbContext context = OfflineDbContextFactory.Create();

        string sql = context.Users.ForOrganization(Guid.CreateVersion7()).ToQueryString();

        sql.Should().MatchRegex(@"WHERE u\.organization_id = @\w+");
    }

    [Fact]
    public void ForOrganization_WhenOrganizationIsEmpty_ThrowsWithEntityName()
    {
        using AppDbContext context = OfflineDbContextFactory.Create();

        Action act = () => context.Users.ForOrganization(Guid.Empty);

        act.Should().Throw<ArgumentException>().WithMessage("*User*");
    }
}
