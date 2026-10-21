namespace FleetLedger.Api.Data.Seed;

public static class AdministratorSeedExtension
{
    public static void AddAdministratorSeed(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IAdministratorStore, IdentityAdministratorStore>();
        builder.Services.AddScoped<AdministratorSeeder>();
    }

    // Roda depois das migrations: precisa das tabelas users e organizations.
    public static async Task SeedAdministratorAsync(this WebApplication app)
    {
        await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
        AdministratorSeeder seeder = scope.ServiceProvider.GetRequiredService<AdministratorSeeder>();
        await seeder.SeedAsync(app.Lifetime.ApplicationStopping);
    }
}
