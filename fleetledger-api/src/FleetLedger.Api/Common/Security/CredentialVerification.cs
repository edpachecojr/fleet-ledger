namespace FleetLedger.Api.Common.Security;

public sealed record CredentialVerification(Guid UserId, ELoginRefusal Refusal)
{
    public bool IsVerified => Refusal == ELoginRefusal.None;

    public static CredentialVerification Verified(Guid userId) => new(userId, ELoginRefusal.None);

    public static CredentialVerification Refused(ELoginRefusal refusal) => new(Guid.Empty, refusal);
}
