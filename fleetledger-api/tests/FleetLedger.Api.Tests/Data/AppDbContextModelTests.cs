using FleetLedger.Api.Data;
using FleetLedger.Api.Data.Seed;
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
            .BeEquivalentTo(
                "organizations", "users", "user_claims", "user_logins", "user_tokens",
                "ledger_account_groups", "ledger_accounts", "vehicles", "customers", "drivers");

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

    [Fact]
    public void ChartOfAccounts_IsSeededByTheMigrationWithoutOrganization()
    {
        IEntityType account = Entity<LedgerAccount>();

        account.GetSeedData().Should().HaveCount(ChartOfAccountsCatalog.Accounts.Count);
        Entity<LedgerAccountGroup>().GetSeedData().Should().HaveCount(ChartOfAccountsCatalog.Groups.Count);
        account.FindProperty(nameof(IOrganizationOwned.OrganizationId)).Should().BeNull();
    }

    [Fact]
    public void Vehicle_PlateIsUniquePerOrganization()
        => Entity<Vehicle>().GetIndexes().Should().Contain(index =>
            index.IsUnique && index.GetDatabaseName() == "ix_vehicles_organization_id_plate");

    [Fact]
    public void Customer_CnpjIsUniquePerOrganization()
        => Entity<Customer>().GetIndexes().Should().Contain(index =>
            index.IsUnique && index.GetDatabaseName() == "ix_customers_organization_id_cnpj");

    [Theory]
    [InlineData("ix_drivers_organization_id_cpf")]
    [InlineData("ix_drivers_organization_id_cnh_number")]
    public void Driver_CpfAndCnhAreUniquePerOrganization(string indexName)
        => Entity<Driver>().GetIndexes().Should().Contain(index => index.IsUnique && index.GetDatabaseName() == indexName);

    [Fact]
    public void Driver_DocumentStatusIsCalculatedAndNotStored()
        => Entity<Driver>().FindProperty(nameof(Driver.DocumentStatus)).Should().BeNull();

    [Fact]
    public void Vehicle_IsActiveIsDerivedAndNotStored()
        => Entity<Vehicle>().FindProperty(nameof(Vehicle.IsActive)).Should().BeNull();

    private IEntityType Entity<TEntity>() => _model.FindEntityType(typeof(TEntity))!;

    private static IModel DesignTimeModel()
    {
        using AppDbContext context = OfflineDbContextFactory.Create();
        return context.GetService<IDesignTimeModel>().Model;
    }
}
