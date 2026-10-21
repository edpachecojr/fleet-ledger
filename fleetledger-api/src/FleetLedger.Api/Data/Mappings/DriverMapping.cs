using FleetLedger.Core.Models;
using FleetLedger.Core.Validators.Documents;
using FleetLedger.Core.Validators.Drivers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetLedger.Api.Data.Mappings;

public sealed class DriverMapping : IEntityTypeConfiguration<Driver>
{
    // Comissão até 100,00%: 3 inteiros + 2 casas.
    private const int CommissionPrecision = 5;

    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("drivers");
        builder.Property(driver => driver.Name).HasMaxLength(DriverProfileRules.NameMaxLength).IsRequired();
        builder.Property(driver => driver.Cpf).HasMaxLength(CpfNumber.Length).IsFixedLength().IsRequired();
        builder.Property(driver => driver.Phone).HasMaxLength(PhoneNumber.MobileLength).IsRequired();
        builder.Property(driver => driver.CnhNumber).HasMaxLength(DriverProfileRules.CnhNumberLength).IsFixedLength().IsRequired();
        builder.Property(driver => driver.DefaultCommissionPercent)
            .HasPrecision(CommissionPrecision, DriverProfileRules.CommissionDecimalPlaces);
        builder.Property(driver => driver.BankDetails).HasMaxLength(DriverProfileRules.BankDetailsMaxLength);
        builder.Property(driver => driver.DeactivatedAt).HasPrecision(6);

        // Calculada a cada leitura com a data de hoje (DriverDocumentStatusRule).
        builder.Ignore(driver => driver.DocumentStatus);

        // CPF e CNH únicos por organização, inclusive entre inativos (TASK-007).
        builder.HasIndex(driver => new { driver.OrganizationId, driver.Cpf }).IsUnique();
        builder.HasIndex(driver => new { driver.OrganizationId, driver.CnhNumber }).IsUnique();
    }
}
