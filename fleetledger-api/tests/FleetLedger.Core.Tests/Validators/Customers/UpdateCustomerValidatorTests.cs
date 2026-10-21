using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Customers;
using FleetLedger.Core.Validators.Customers;

namespace FleetLedger.Core.Tests.Validators.Customers;

public class UpdateCustomerValidatorTests
{
    private readonly UpdateCustomerValidator _validator = new();

    [Fact]
    public void Validate_WithValidProfile_ReturnsValid()
        => _validator.Validate(new UpdateCustomerRequest { CorporateName = "Agro Sul Ltda", BillingCycle = EBillingCycle.Biweekly })
            .IsValid.Should().BeTrue();

    [Fact]
    public void Validate_AppliesTheSameProfileRulesAsTheCreation()
        => _validator.Validate(new UpdateCustomerRequest { CorporateName = "", BillingCycle = 0, Email = "x" })
            .Errors.Select(error => error.Field).Should().BeEquivalentTo("CorporateName", "BillingCycle", "Email");
}
