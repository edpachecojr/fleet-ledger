using FleetLedger.Api.Data.Seed;
using FleetLedger.Api.Models;
using FleetLedger.Core.Models;

namespace FleetLedger.Api.Tests.Fakes;

public sealed class FakeAdministratorStore : IAdministratorStore
{
    private bool _hasUsers;

    public Organization? CreatedOrganization { get; private set; }
    public User? CreatedAdministrator { get; private set; }
    public string? CreatedWithPassword { get; private set; }

    public FakeAdministratorStore WithExistingUser()
    {
        _hasUsers = true;
        return this;
    }

    public Task<bool> HasAnyUserAsync(CancellationToken cancellationToken) => Task.FromResult(_hasUsers);

    public Task CreateAsync(Organization organization, User administrator, string password)
    {
        CreatedOrganization = organization;
        CreatedAdministrator = administrator;
        CreatedWithPassword = password;
        _hasUsers = true;
        return Task.CompletedTask;
    }
}
