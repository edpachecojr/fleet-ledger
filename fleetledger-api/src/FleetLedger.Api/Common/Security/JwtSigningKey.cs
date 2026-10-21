using System.Text;
using FleetLedger.Api.Options;
using Microsoft.IdentityModel.Tokens;

namespace FleetLedger.Api.Common.Security;

// Emissão e validação precisam derivar a chave do mesmo jeito; um único ponto garante isso.
public static class JwtSigningKey
{
    public static SymmetricSecurityKey From(JwtOptions options)
        => new(Encoding.UTF8.GetBytes(options.SigningKey));
}
