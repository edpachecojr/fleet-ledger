using FleetLedger.Api.Data.Seed;
using FleetLedger.Api.Options;
using FleetLedger.Api.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace FleetLedger.Api.Tests.Data.Seed;

public class AdministratorSeederTests
{
    private static readonly AdministratorSeedOptions Seed = new()
    {
        OrganizationName = "Transportes Exemplo",
        Email = "gestor@fleetledger.dev",
        Password = "Senha-Forte-123"
    };

    [Fact]
    public async Task SeedAsync_OnEmptyDatabase_CreatesAdministratorInsideTheNewOrganization()
    {
        FakeAdministratorStore store = new();

        await SeederWith(store).SeedAsync(CancellationToken.None);

        store.CreatedAdministrator!.OrganizationId.Should().Be(store.CreatedOrganization!.Id);
    }

    [Fact]
    public async Task SeedAsync_OnEmptyDatabase_UsesConfiguredValues()
    {
        FakeAdministratorStore store = new();

        await SeederWith(store).SeedAsync(CancellationToken.None);

        new { store.CreatedOrganization!.Name, store.CreatedAdministrator!.Email, Password = store.CreatedWithPassword }
            .Should().BeEquivalentTo(new { Name = Seed.OrganizationName, Seed.Email, Seed.Password });
    }

    [Fact]
    public async Task SeedAsync_OnEmptyDatabase_StampsOrganizationWithCurrentTime()
    {
        FakeAdministratorStore store = new();

        await SeederWith(store).SeedAsync(CancellationToken.None);

        store.CreatedOrganization!.CreatedAt.Should().Be(FakeTimeProvider.ReferenceNow);
    }

    [Fact]
    public async Task SeedAsync_WhenAnyUserExists_CreatesNothing()
    {
        FakeAdministratorStore store = new FakeAdministratorStore().WithExistingUser();

        await SeederWith(store).SeedAsync(CancellationToken.None);

        store.CreatedAdministrator.Should().BeNull();
    }

    private static AdministratorSeeder SeederWith(FakeAdministratorStore store)
        => new(store, OptionsFactory.Create(Seed), new FakeTimeProvider(), NullLogger<AdministratorSeeder>.Instance);
}
