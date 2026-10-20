using FleetLedger.Api.Data;
using FleetLedger.Api.Models;
using FleetLedger.Api.Tests.Fixtures;
using FleetLedger.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace FleetLedger.Api.Tests.Data;

public class AppDbContextModelTests
{
    private readonly IModel _model = DesignTimeModel();

    [Fact]
    public void Tables_ForEveryEntity_AreSnakeCasePluralWithoutAspNetPrefix()
        => _model.GetEntityTypes().Select(entity => entity.GetTableName()).Should()
            .BeEquivalentTo("organizations", "users", "user_claims", "user_logins", "user_tokens");

    [Fact]
    public void Columns_ForBaseModel_AreSnakeCase()
        => Entity<Organization>().GetProperties().Select(property => property.GetColumnName()).Should()
            .Contain(["id", "created_at", "updated_at"]);

    [Fact]
    public void Id_ForBaseModel_IsPrimaryKeyNeverGeneratedByDatabase()
    {
        IEntityType organization = Entity<Organization>();

        organization.FindPrimaryKey()!.Properties.Should().ContainSingle(property => property.Name == nameof(BaseModel.Id));
        organization.FindProperty(nameof(BaseModel.Id))!.ValueGenerated.Should().Be(ValueGenerated.Never);
    }

    [Theory]
    [InlineData(nameof(BaseModel.CreatedAt))]
    [InlineData(nameof(BaseModel.UpdatedAt))]
    public void AuditDates_ForBaseModel_UseMicrosecondPrecision(string propertyName)
        => Entity<Organization>().FindProperty(propertyName)!.GetPrecision().Should().Be(6);

    [Fact]
    public void OrganizationId_ForOrganizationOwnedEntity_IsIndexedForeignKey()
    {
        IEntityType user = Entity<User>();

        user.GetIndexes().Should().Contain(index => index.GetDatabaseName() == "ix_users_organization_id");
        user.GetForeignKeys().Should().Contain(foreignKey => foreignKey.PrincipalEntityType.ClrType == typeof(Organization));
    }

    [Fact]
    public void ForeignKeys_ForIdentityTables_ReferenceRenamedUsersTable()
        => _model.GetEntityTypes().SelectMany(entity => entity.GetForeignKeys())
            .Select(foreignKey => foreignKey.GetConstraintName())
            .Should().NotContain(name => name!.Contains("asp_net"));

    private IEntityType Entity<TEntity>() => _model.FindEntityType(typeof(TEntity))!;

    private static IModel DesignTimeModel()
    {
        using AppDbContext context = OfflineDbContextFactory.Create();
        return context.GetService<IDesignTimeModel>().Model;
    }
}
