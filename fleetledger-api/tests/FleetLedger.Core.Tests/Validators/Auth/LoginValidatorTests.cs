using FleetLedger.Core.Common.Validation;
using FleetLedger.Core.Requests.Auth;
using FleetLedger.Core.Validators.Auth;

namespace FleetLedger.Core.Tests.Validators.Auth;

public class LoginValidatorTests
{
    private readonly LoginValidator _validator = new();

    [Fact]
    public void Validate_WhenEmailAndPasswordAreFilled_ReturnsValid()
        => _validator.Validate(ValidRequest()).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_WhenEmailIsBlank_ReturnsEmailError()
        => FieldsWithError(ValidRequest(email: " ")).Should().ContainSingle().Which.Should().Be("Email");

    [Fact]
    public void Validate_WhenEmailIsTooLong_ReturnsEmailError()
        => FieldsWithError(ValidRequest(email: new string('a', LoginValidator.EmailMaxLength) + "@x.com"))
            .Should().ContainSingle().Which.Should().Be("Email");

    [Fact]
    public void Validate_WhenPasswordIsEmpty_ReturnsPasswordError()
        => FieldsWithError(ValidRequest(password: "")).Should().ContainSingle().Which.Should().Be("Password");

    [Fact]
    public void Validate_WhenPasswordIsTooLong_ReturnsPasswordError()
        => FieldsWithError(ValidRequest(password: new string('x', LoginValidator.PasswordMaxLength + 1)))
            .Should().ContainSingle().Which.Should().Be("Password");

    private IEnumerable<string> FieldsWithError(LoginRequest request)
        => _validator.Validate(request).Errors.Select((ValidationError error) => error.Field);

    private static LoginRequest ValidRequest(string email = "gestor@fleetledger.dev", string password = "Senha-Forte-123")
        => new() { Email = email, Password = password };
}
