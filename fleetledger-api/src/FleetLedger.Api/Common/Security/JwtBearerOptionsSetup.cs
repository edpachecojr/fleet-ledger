using FleetLedger.Api.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FleetLedger.Api.Common.Security;

public sealed class JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions) : IConfigureNamedOptions<JwtBearerOptions>
{
    private static readonly TimeSpan AllowedClockSkew = TimeSpan.FromSeconds(30);

    public void Configure(string? name, JwtBearerOptions options)
    {
        if (name != JwtBearerDefaults.AuthenticationScheme)
            return;

        Configure(options);
    }

    public void Configure(JwtBearerOptions options)
    {
        JwtOptions jwt = jwtOptions.Value;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = JwtSigningKey.From(jwt),
            ValidateLifetime = true,
            ClockSkew = AllowedClockSkew
        };
    }
}
