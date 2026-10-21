namespace FleetLedger.Api.Common.Security;

public interface ICredentialVerifier
{
    // Sem CancellationToken: o UserManager do Identity não aceita um por chamada.
    Task<CredentialVerification> VerifyAsync(string email, string password);
}
