using FleetLedger.Core.Responses.Auth;

namespace FleetLedger.Api.Common.Security;

public interface IAccessTokenIssuer
{
    IssuedAccessToken Issue(Guid userId);
}
