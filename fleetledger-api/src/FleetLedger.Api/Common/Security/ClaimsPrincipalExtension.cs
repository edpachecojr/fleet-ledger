using System.Security.Claims;

namespace FleetLedger.Api.Common.Security;

public static class ClaimsPrincipalExtension
{
    // Lança em vez de devolver Guid.Empty: um id vazio removeria o filtro da consulta.
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        string? claimValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(claimValue))
            throw new UnauthorizedAccessException(
                $"Claim '{ClaimTypes.NameIdentifier}' ausente no access token. " +
                "Esperado: o id (Guid) do usuário na claim 'sub'.");

        if (!Guid.TryParse(claimValue, out Guid userId) || userId == Guid.Empty)
            throw new UnauthorizedAccessException(
                $"Claim '{ClaimTypes.NameIdentifier}' inválida. Recebido: '{claimValue}'. Esperado: Guid não vazio.");

        return userId;
    }
}
