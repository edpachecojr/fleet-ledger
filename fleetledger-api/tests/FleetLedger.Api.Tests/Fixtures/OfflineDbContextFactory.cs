using FleetLedger.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Tests.Fixtures;

// Contexto configurado como em produção, mas que nunca abre conexão: serve para
// inspecionar o modelo e o SQL gerado sem depender de banco.
public static class OfflineDbContextFactory
{
    private const string UnreachableConnectionString = "Host=unreachable.invalid;Database=fleetledger_tests";

    public static AppDbContext Create()
    {
        DbContextOptionsBuilder<AppDbContext> options = new();
        DataExtension.UsePostgres(options, UnreachableConnectionString);
        return new AppDbContext(options.Options);
    }
}
