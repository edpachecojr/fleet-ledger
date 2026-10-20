using FleetLedger.Api.Data;

namespace FleetLedger.Api.Tests.Fakes;

public sealed class FakeUserOrganizationReader : IUserOrganizationReader
{
    private readonly Dictionary<Guid, Guid> _organizationByUser = [];

    public int CallCount { get; private set; }

    public FakeUserOrganizationReader WithUser(Guid userId, Guid organizationId)
    {
        _organizationByUser[userId] = organizationId;
        return this;
    }

    public Task<Guid?> FindOrganizationIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        CallCount++;
        Guid? organizationId = _organizationByUser.TryGetValue(userId, out Guid found) ? found : null;
        return Task.FromResult(organizationId);
    }
}
