using FleetLedger.Api.Common.Api;

namespace FleetLedger.Api.Endpoints.Auth;

public static class AuthEndpointGroup
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
        => app.MapGroup("v1/auth")
            .WithTags("Auth")
            .MapEndpoint<LoginEndpoint>();
}
