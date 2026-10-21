using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Drivers;
using FleetLedger.Core.Validators.Drivers;

namespace FleetLedger.Core.Tests.Validators.Drivers;

public class UpdateDriverValidatorTests
{
    private readonly UpdateDriverValidator _validator = new();

    [Fact]
    public void Validate_WithValidProfile_ReturnsValid()
        => _validator.Validate(ValidRequest()).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_AppliesTheSameProfileRulesAsTheCreation()
    {
        UpdateDriverRequest request = ValidRequest();
        request.Name = "";
        request.DefaultCommissionPercent = 150m;

        _validator.Validate(request).Errors.Select(error => error.Field).Should()
            .BeEquivalentTo("Name", "DefaultCommissionPercent");
    }

    private static UpdateDriverRequest ValidRequest()
        => new()
        {
            Name = "João da Silva",
            Phone = "11987654321",
            CnhNumber = "12345678901",
            CnhCategory = ECnhCategory.C,
            CnhExpiresOn = new DateOnly(2028, 5, 10),
            DefaultCommissionPercent = 10m
        };
}
