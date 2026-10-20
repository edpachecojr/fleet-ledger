using FleetLedger.Api.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Data;

public static class DataExtension
{
    private const string MigrationsHistoryTable = "__ef_migrations_history";

    public static void AddDataContexts(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>((services, options) =>
            UsePostgres(options, services.GetRequiredService<IOptions<DatabaseOptions>>().Value.ConnectionString));

        builder.Services.AddScoped<IUserOrganizationReader, UserOrganizationReader>();
    }

    // O ÚNICO ponto que nomeia o banco (docs/adr/0001). Os testes usam o mesmo
    // método, então o modelo verificado é o mesmo que vai para produção.
    public static DbContextOptionsBuilder UsePostgres(DbContextOptionsBuilder options, string connectionString)
        => options
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsHistoryTable(MigrationsHistoryTable))
            .UseSnakeCaseNamingConvention();

    public static async Task ApplyPendingMigrationsAsync(this WebApplication app)
    {
        DatabaseOptions options = app.Services.GetRequiredService<IOptions<DatabaseOptions>>().Value;
        if (!options.ApplyMigrationsOnStartup)
            return;

        await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync(app.Lifetime.ApplicationStopping);

        app.Logger.LogInformation("Migrations pendentes aplicadas na inicialização");
    }
}
