using Scalar.AspNetCore;

namespace FleetLedger.Api.Common.Api;

public static class DevelopmentEnvironmentExtension
{
    // A UI interativa fica só em desenvolvimento; o documento OpenAPI, não.
    public static void ConfigureDevEnvironment(this WebApplication app)
        => app.MapScalarApiReference().AllowAnonymous();
}
