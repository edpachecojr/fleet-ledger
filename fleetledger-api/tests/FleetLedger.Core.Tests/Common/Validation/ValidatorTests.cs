using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Tests.Fakes;

namespace FleetLedger.Core.Tests.Common.Validation;

public class ValidatorTests
{
    private readonly SampleRequestValidator _validator = new();

    [Fact]
    public void Validate_WhenRulesPass_ReturnsValid()
        => _validator.Validate(new SampleRequest { Name = "FleetLedger" }).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_WhenRuleFails_ReturnsFieldError()
    {
        ValidationResult result = _validator.Validate(new SampleRequest { Name = "" });

        result.Errors.Should().ContainSingle(error => error.Field == nameof(SampleRequest.Name));
    }
}
