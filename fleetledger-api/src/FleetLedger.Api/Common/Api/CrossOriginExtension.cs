using FleetLedger.Api.Options;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Common.Api;

public static class CrossOriginExtension
{
    // Sem AllowCredentials: a autenticação é por bearer token, não por cookie.
    public static void AddCrossOrigin(this WebApplicationBuilder builder)
    {
        builder.Services.AddCors();
        builder.Services
            .AddOptions<CorsOptions>()
            .Configure<IOptions<CrossOriginOptions>>((cors, crossOrigin) => cors.AddPolicy(
                CorsPolicies.AllowedClients,
                policy => policy
                    .WithOrigins([.. crossOrigin.Value.AllowedOrigins])
                    .AllowAnyHeader()
                    .AllowAnyMethod()));
    }
}
