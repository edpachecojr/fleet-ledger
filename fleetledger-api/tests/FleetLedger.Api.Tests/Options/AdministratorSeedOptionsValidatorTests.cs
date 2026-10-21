using FleetLedger.Api.Options;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Tests.Options;

public class AdministratorSeedOptionsValidatorTests
{
    private const string ValidPassword = "Senha-Forte-123";

    private readonly AdministratorSeedOptionsValidator _validator = new();

    [Fact]
    public void Validate_WhenAllValuesAreSet_Succeeds()
        => _validator.Validate(null, ValidOptions()).Succeeded.Should().BeTrue();

    [Fact]
    public void Validate_WhenOrganizationNameIsBlank_FailsNamingTheKey()
        => _validator.Validate(null, ValidOptions(organizationName: ""))
            .FailureMessage.Should().Contain("AdministratorSeed:OrganizationName");

    [Fact]
    public void Validate_WhenOrganizationNameIsTooLong_FailsWithReceivedLength()
        => _validator.Validate(null, ValidOptions(organizationName: new string('a', 161)))
            .FailureMessage.Should().Contain("AdministratorSeed:OrganizationName").And.Contain("161");

    [Theory]
    [InlineData("")]
    [InlineData("gestor")]
    [InlineData("@fleetledger.dev")]
    [InlineData("gestor@")]
    public void Validate_WhenEmailIsNotAnEmail_FailsWithReceivedValue(string email)
        => _validator.Validate(null, ValidOptions(email: email))
            .FailureMessage.Should().Contain("AdministratorSeed:Email").And.Contain($"'{email}'");

    [Fact]
    public void Validate_WhenPasswordIsShort_FailsWithLengthButNotThePassword()
    {
        ValidateOptionsResult result = _validator.Validate(null, ValidOptions(password: "Curta-1"));

        result.FailureMessage.Should().Contain("AdministratorSeed:Password").And.Contain("7 caracteres");
        result.FailureMessage.Should().NotContain("Curta-1");
    }

    [Fact]
    public void Validate_WhenPasswordExceedsLoginLimit_Fails()
        => _validator.Validate(null, ValidOptions(password: new string('x', 129)))
            .FailureMessage.Should().Contain("AdministratorSeed:Password");

    private static AdministratorSeedOptions ValidOptions(
        string organizationName = "Transportes Exemplo",
        string email = "gestor@fleetledger.dev",
        string password = ValidPassword)
        => new() { OrganizationName = organizationName, Email = email, Password = password };
}
