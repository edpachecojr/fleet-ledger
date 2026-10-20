using System.Security.Claims;
using FleetLedger.Api.Data;
using FleetLedger.Core.Security;
using Microsoft.Extensions.Caching.Hybrid;

namespace FleetLedger.Api.Common.Security;

public sealed class UserContext(
    IHttpContextAccessor httpContextAccessor,
    IUserOrganizationReader userOrganizationReader,
    HybridCache cache) : IUserContext
{
    private const string OrganizationCacheKeyPrefix = "user-organization:";

    private static readonly HybridCacheEntryOptions OrganizationCacheEntry = new()
    {
        Expiration = TimeSpan.FromMinutes(5),
        LocalCacheExpiration = TimeSpan.FromMinutes(5)
    };

    public Guid UserId => CurrentUser.GetUserId();

    public async Task<Guid> GetOrganizationIdAsync(CancellationToken cancellationToken)
    {
        Guid userId = UserId;

        return await cache.GetOrCreateAsync(
            OrganizationCacheKeyPrefix + userId,
            userId,
            FindOrganizationIdAsync,
            OrganizationCacheEntry,
            cancellationToken: cancellationToken);
    }

    // A exceção sai de dentro da fábrica: o HybridCache não guarda falha, então um
    // usuário criado depois é encontrado na próxima requisição.
    private async ValueTask<Guid> FindOrganizationIdAsync(Guid userId, CancellationToken cancellationToken)
        => await userOrganizationReader.FindOrganizationIdAsync(userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

    private ClaimsPrincipal CurrentUser
        => httpContextAccessor.HttpContext?.User
            ?? throw new InvalidOperationException(
                $"{nameof(IUserContext)} usado fora de uma requisição HTTP. " +
                "Esperado: chamada a partir de um endpoint, com HttpContext ativo.");
}
