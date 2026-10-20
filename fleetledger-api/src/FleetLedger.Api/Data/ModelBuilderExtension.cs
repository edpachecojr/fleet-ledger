using FleetLedger.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetLedger.Api.Data;

// Convenções aplicadas uma única vez a todo o modelo, para que nenhum mapeamento
// de entidade precise repeti-las (nem possa esquecê-las).
public static class ModelBuilderExtension
{
    // Microssegundos: a maior precisão do timestamptz do PostgreSQL.
    private const int TimestampPrecision = 6;

    public static ModelBuilder ApplyBaseModelConfiguration(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entityType in RootEntityTypesAssignableTo<BaseModel>(modelBuilder))
            ConfigureBaseModel(modelBuilder.Entity(entityType.ClrType));

        return modelBuilder;
    }

    public static ModelBuilder ApplyOrganizationOwnership(this ModelBuilder modelBuilder)
    {
        foreach (IMutableEntityType entityType in RootEntityTypesAssignableTo<IOrganizationOwned>(modelBuilder))
            ConfigureOrganizationOwnership(modelBuilder.Entity(entityType.ClrType));

        return modelBuilder;
    }

    private static void ConfigureBaseModel(EntityTypeBuilder entity)
    {
        entity.HasKey(nameof(BaseModel.Id));
        entity.Property(nameof(BaseModel.Id)).ValueGeneratedNever();
        entity.Property(nameof(BaseModel.CreatedAt)).HasPrecision(TimestampPrecision).IsRequired();
        entity.Property(nameof(BaseModel.UpdatedAt)).HasPrecision(TimestampPrecision).IsRequired();
    }

    // Índice simples por organização; índice único composto, quando a cardinalidade
    // exigir, fica no mapeamento da própria entidade.
    private static void ConfigureOrganizationOwnership(EntityTypeBuilder entity)
    {
        entity.HasOne(typeof(Organization))
            .WithMany()
            .HasForeignKey(nameof(IOrganizationOwned.OrganizationId))
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasIndex(nameof(IOrganizationOwned.OrganizationId));
    }

    // Materializada porque configurar a entidade altera a coleção de tipos do modelo.
    private static List<IMutableEntityType> RootEntityTypesAssignableTo<TContract>(ModelBuilder modelBuilder)
        => modelBuilder.Model.GetEntityTypes()
            .Where(entityType => entityType.BaseType is null)
            .Where(entityType => typeof(TContract).IsAssignableFrom(entityType.ClrType))
            .ToList();
}
