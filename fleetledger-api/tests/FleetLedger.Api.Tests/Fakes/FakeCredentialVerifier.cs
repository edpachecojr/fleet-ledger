using FleetLedger.Api.Common.Security;

namespace FleetLedger.Api.Tests.Fakes;

public sealed class FakeCredentialVerifier(CredentialVerification verification) : ICredentialVerifier
{
    public Task<CredentialVerification> VerifyAsync(string email, string password)
        => Task.FromResult(verification);
}
