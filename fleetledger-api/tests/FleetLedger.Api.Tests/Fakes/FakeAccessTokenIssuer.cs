using FleetLedger.Api.Common.Security;
using FleetLedger.Core.Responses.Auth;

namespace FleetLedger.Api.Tests.Fakes;

public sealed class FakeAccessTokenIssuer : IAccessTokenIssuer
{
    public List<Guid> IssuedFor { get; } = [];

    public IssuedAccessToken Issue(Guid userId)
    {
        IssuedFor.Add(userId);
        return new IssuedAccessToken($"fake-token-for-{userId}", FakeTimeProvider.ReferenceNow.AddHours(8));
    }
}
