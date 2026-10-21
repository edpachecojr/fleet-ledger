using FleetLedger.Api.Data.Mappings.Identity;
using FleetLedger.Api.Models;
using FleetLedger.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FleetLedger.Api.Data;

// IdentityUserContext (sem roles): o MVP tem um único perfil, o administrador.
public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityUserContext<User, Guid>(options)
{
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<LedgerAccountGroup> LedgerAccountGroups => Set<LedgerAccountGroup>();
    public DbSet<LedgerAccount> LedgerAccounts => Set<LedgerAccount>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Driver> Drivers => Set<Driver>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // users é renomeada antes das tabelas dependentes: a convenção snake_case nomeia
        // as FKs com o nome da tabela principal vigente no momento (senão sai asp_net_users).
        builder.ApplyConfiguration(new UserMapping());
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        builder.ApplyBaseModelConfiguration();
        builder.ApplyOrganizationOwnership();
    }
}
