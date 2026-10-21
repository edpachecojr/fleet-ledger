using FleetLedger.Core.Models;
using FleetLedger.Core.Validators.Customers;
using FleetLedger.Core.Validators.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FleetLedger.Api.Data.Mappings;

public sealed class CustomerMapping : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");
        builder.Property(customer => customer.CorporateName).HasMaxLength(CustomerProfileRules.NameMaxLength).IsRequired();
        builder.Property(customer => customer.TradeName).HasMaxLength(CustomerProfileRules.NameMaxLength);
        builder.Property(customer => customer.Cnpj).HasMaxLength(CnpjNumber.Length).IsFixedLength().IsRequired();
        builder.Property(customer => customer.Phone).HasMaxLength(PhoneNumber.MobileLength);
        builder.Property(customer => customer.Email).HasMaxLength(CustomerProfileRules.EmailMaxLength);
        builder.Property(customer => customer.DeactivatedAt).HasPrecision(6);

        // CNPJ único por organização, inclusive entre inativos (TASK-006).
        builder.HasIndex(customer => new { customer.OrganizationId, customer.Cnpj }).IsUnique();
    }
}
