using FleetLedger.Api.Models;
using FleetLedger.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Data.Seed;

public sealed class IdentityAdministratorStore(AppDbContext context, UserManager<User> userManager)
    : IAdministratorStore
{
    public async Task<bool> HasAnyUserAsync(CancellationToken cancellationToken)
        => await context.Users.AnyAsync(cancellationToken);

    // A organização só é rastreada aqui; o SaveChanges do UserManager.CreateAsync grava
    // as duas linhas juntas, então não sobra organização órfã se o usuário falhar.
    public async Task CreateAsync(Organization organization, User administrator, string password)
    {
        context.Organizations.Add(organization);

        IdentityResult result = await userManager.CreateAsync(administrator, password);
        if (result.Succeeded)
            return;

        context.Entry(organization).State = EntityState.Detached;
        throw new InvalidOperationException(
            $"Não foi possível criar o administrador '{administrator.Email}': " +
            $"{string.Join("; ", result.Errors.Select(error => error.Description))}. " +
            "Esperado: AdministratorSeed:Password dentro da política do Identity " +
            "(maiúscula, minúscula, dígito e símbolo).");
    }
}
