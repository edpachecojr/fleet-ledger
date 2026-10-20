using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace FleetLedger.Api.Tests.Fixtures;

public static class HttpContextAccessorFactory
{
    public static IHttpContextAccessor WithUser(Guid userId)
        => WithClaims(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));

    public static IHttpContextAccessor WithClaims(params Claim[] claims)
        => new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test")) }
        };

    public static IHttpContextAccessor WithoutRequest() => new HttpContextAccessor();
}
