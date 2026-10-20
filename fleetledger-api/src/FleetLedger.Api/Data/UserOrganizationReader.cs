using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Data;

public sealed class UserOrganizationReader(AppDbContext context) : IUserOrganizationReader
{
    // Única leitura de entidade IOrganizationOwned sem ForOrganization: é ela que
    // descobre a organização, a partir do usuário do access token.
    public async Task<Guid?> FindOrganizationIdAsync(Guid userId, CancellationToken cancellationToken)
        => await context.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .Select(user => (Guid?)user.OrganizationId)
            .SingleOrDefaultAsync(cancellationToken);
}
