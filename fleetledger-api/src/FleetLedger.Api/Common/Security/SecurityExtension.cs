using FleetLedger.Api.Data;
using FleetLedger.Api.Models;
using FleetLedger.Core.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace FleetLedger.Api.Common.Security;

public static class SecurityExtension
{
    public static void AddSecurity(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddIdentityCore<User>(AdministratorIdentityPolicy.Apply)
            .AddEntityFrameworkStores<AppDbContext>();

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
        builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();

        // Fechado por padrão: rota nova exige autenticação sem ninguém lembrar de pedir.
        // Exceções explícitas usam AllowAnonymous (health, login, documento OpenAPI).
        builder.Services
            .AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<IUserContext, UserContext>();
        builder.Services.AddScoped<ICredentialVerifier, IdentityCredentialVerifier>();
        builder.Services.AddSingleton<IAccessTokenIssuer, AccessTokenIssuer>();
    }

    public static void UseSecurity(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
