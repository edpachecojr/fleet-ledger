using FleetLedger.Api.Data.Seed;
using FleetLedger.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetLedger.Api.Data.Mappings;

// Global, sem organização: o Plano de Contas é o mesmo para todas (decisão de 2026-10-21).
public sealed class LedgerAccountMapping : IEntityTypeConfiguration<LedgerAccount>
{
    public const int CodeMaxLength = 8;
    public const int NameMaxLength = 80;

    public void Configure(EntityTypeBuilder<LedgerAccount> builder)
    {
        builder.ToTable("ledger_accounts");
        builder.Property(account => account.Code).HasMaxLength(CodeMaxLength).IsRequired();
        builder.Property(account => account.Name).HasMaxLength(NameMaxLength).IsRequired();
        builder.HasIndex(account => account.Code).IsUnique();
        builder.HasIndex(account => account.EntryContext);

        builder.HasData(ChartOfAccountsCatalog.Accounts);
    }
}
