using FleetLedger.Api.Handlers.Auth;
using FleetLedger.Core.Handlers;

namespace FleetLedger.Api.Common.Api;

public static class ApplicationServicesExtension
{
    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddHybridCache();

        builder.Services.AddScoped<IAuthHandler, AuthHandler>();
    }
}
