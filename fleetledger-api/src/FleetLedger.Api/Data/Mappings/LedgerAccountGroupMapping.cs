using FleetLedger.Api.Data.Seed;
using FleetLedger.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetLedger.Api.Data.Mappings;

public sealed class LedgerAccountGroupMapping : IEntityTypeConfiguration<LedgerAccountGroup>
{
    public const int CodeMaxLength = 8;
    public const int NameMaxLength = 80;

    public void Configure(EntityTypeBuilder<LedgerAccountGroup> builder)
    {
        builder.ToTable("ledger_account_groups");
        builder.Property(group => group.Code).HasMaxLength(CodeMaxLength).IsRequired();
        builder.Property(group => group.Name).HasMaxLength(NameMaxLength).IsRequired();
        builder.HasIndex(group => group.Code).IsUnique();

        builder.HasMany(group => group.Accounts)
            .WithOne()
            .HasForeignKey(account => account.GroupId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasData(ChartOfAccountsCatalog.Groups);
    }
}
