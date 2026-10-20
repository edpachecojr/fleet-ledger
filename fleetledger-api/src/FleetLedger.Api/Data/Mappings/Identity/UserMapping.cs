using FleetLedger.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetLedger.Api.Data.Mappings.Identity;

// Substitui os nomes padrão do Identity (AspNetUsers, EmailIndex, UserNameIndex) pelo padrão do projeto.
public sealed class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        builder.Property(user => user.Id).ValueGeneratedNever();
        builder.HasIndex(user => user.NormalizedEmail).HasDatabaseName("ix_users_normalized_email");
        builder.HasIndex(user => user.NormalizedUserName).HasDatabaseName("ix_users_normalized_user_name");
    }
}
