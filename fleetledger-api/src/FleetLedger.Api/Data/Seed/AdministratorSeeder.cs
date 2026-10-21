using FleetLedger.Api.Models;
using FleetLedger.Api.Options;
using FleetLedger.Core.Models;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Data.Seed;

// Idempotente: com qualquer usuário no banco não faz nada. Assim o MVP mantém um
// único administrador e trocar a configuração depois não cria um segundo nem
// redefine a senha de quem já existe.
public sealed class AdministratorSeeder(
    IAdministratorStore administratorStore,
    IOptions<AdministratorSeedOptions> seedOptions,
    TimeProvider timeProvider,
    ILogger<AdministratorSeeder> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken)
    {
        if (await administratorStore.HasAnyUserAsync(cancellationToken))
        {
            logger.LogInformation("Administrador já existe; seed ignorado");
            return;
        }

        AdministratorSeedOptions seed = seedOptions.Value;
        Organization organization = NewOrganization(seed.OrganizationName);
        User administrator = NewAdministrator(seed.Email, organization.Id);

        await administratorStore.CreateAsync(organization, administrator, seed.Password);

        logger.LogInformation(
            "Administrador {UserId} criado na organização {OrganizationId}",
            administrator.Id,
            organization.Id);
    }

    private Organization NewOrganization(string name)
    {
        DateTimeOffset now = timeProvider.GetUtcNow();
        return new Organization { Name = name, CreatedAt = now, UpdatedAt = now };
    }

    private static User NewAdministrator(string email, Guid organizationId)
        => new() { UserName = email, Email = email, EmailConfirmed = true, OrganizationId = organizationId };
}
