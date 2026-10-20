using FleetLedger.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetLedger.Api.Data.Mappings;

public sealed class OrganizationMapping : IEntityTypeConfiguration<Organization>
{
    public const int NameMaxLength = 160;

    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("organizations");
        builder.Property(organization => organization.Name).HasMaxLength(NameMaxLength).IsRequired();
    }
}
