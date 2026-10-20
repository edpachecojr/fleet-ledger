using FleetLedger.Api.Common.Security;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Api.Tests.Fixtures;
using Microsoft.AspNetCore.Http;

namespace FleetLedger.Api.Tests.Common.Security;

public class UserContextTests
{
    private readonly Guid _userId = Guid.CreateVersion7();
    private readonly Guid _organizationId = Guid.CreateVersion7();

    [Fact]
    public void UserId_WhenRequestIsAuthenticated_ReturnsIdFromClaims()
        => CreateContext(new FakeUserOrganizationReader()).UserId.Should().Be(_userId);

    [Fact]
    public void UserId_WhenThereIsNoRequest_ThrowsWithContext()
    {
        UserContext context = CreateContext(new FakeUserOrganizationReader(), HttpContextAccessorFactory.WithoutRequest());

        Action act = () => _ = context.UserId;

        act.Should().Throw<InvalidOperationException>().WithMessage("*fora de uma requisição HTTP*");
    }

    [Fact]
    public async Task GetOrganizationIdAsync_WhenUserExists_ReturnsOrganizationFromReader()
    {
        UserContext context = CreateContext(new FakeUserOrganizationReader().WithUser(_userId, _organizationId));

        Guid organizationId = await context.GetOrganizationIdAsync(CancellationToken.None);

        organizationId.Should().Be(_organizationId);
    }

    [Fact]
    public async Task GetOrganizationIdAsync_WhenCalledTwice_ReadsDatabaseOnce()
    {
        FakeUserOrganizationReader reader = new FakeUserOrganizationReader().WithUser(_userId, _organizationId);
        UserContext context = CreateContext(reader);

        await context.GetOrganizationIdAsync(CancellationToken.None);
        await context.GetOrganizationIdAsync(CancellationToken.None);

        reader.CallCount.Should().Be(1);
    }

    [Fact]
    public async Task GetOrganizationIdAsync_WhenUserDoesNotExist_ThrowsWithUserId()
    {
        UserContext context = CreateContext(new FakeUserOrganizationReader());

        Func<Task> act = () => context.GetOrganizationIdAsync(CancellationToken.None);

        (await act.Should().ThrowAsync<UserNotFoundException>()).WithMessage($"*{_userId}*");
    }

    [Fact]
    public async Task GetOrganizationIdAsync_WhenUserIsCreatedAfterAMiss_FindsItWithoutWaitingForCache()
    {
        FakeUserOrganizationReader reader = new();
        UserContext context = CreateContext(reader);
        await Record.ExceptionAsync(() => context.GetOrganizationIdAsync(CancellationToken.None));
        reader.WithUser(_userId, _organizationId);

        Guid organizationId = await context.GetOrganizationIdAsync(CancellationToken.None);

        organizationId.Should().Be(_organizationId);
    }

    private UserContext CreateContext(FakeUserOrganizationReader reader, IHttpContextAccessor? accessor = null)
        => new(accessor ?? HttpContextAccessorFactory.WithUser(_userId), reader, HybridCacheFactory.Create());
}
