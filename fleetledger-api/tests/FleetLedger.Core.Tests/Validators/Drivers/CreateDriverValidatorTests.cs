using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Drivers;
using FleetLedger.Core.Validators.Drivers;

namespace FleetLedger.Core.Tests.Validators.Drivers;

public class CreateDriverValidatorTests
{
    private readonly CreateDriverValidator _validator = new();

    [Fact]
    public void Validate_WhenEveryFieldIsValid_ReturnsValid()
        => _validator.Validate(ValidRequest()).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_WithoutOptionalFields_ReturnsValid()
        => _validator.Validate(Request(r =>
        {
            r.ToxicologyExamExpiresOn = null;
            r.BankDetails = null;
        })).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("")]
    [InlineData("529.982.247-26")]
    [InlineData("000.000.000-00")]
    public void Validate_WhenCpfIsMissingOrInvalid_ReturnsCpfError(string cpf)
        => FieldsWithError(Request(r => r.Cpf = cpf)).Should().ContainSingle().Which.Should().Be("Cpf");

    [Fact]
    public void Validate_WhenNameIsBlank_ReturnsNameError()
        => FieldsWithError(Request(r => r.Name = "")).Should().ContainSingle().Which.Should().Be("Name");

    [Theory]
    [InlineData("")]
    [InlineData("98765-4321")]
    public void Validate_WhenPhoneIsMissingOrWithoutAreaCode_ReturnsPhoneError(string phone)
        => FieldsWithError(Request(r => r.Phone = phone)).Should().ContainSingle().Which.Should().Be("Phone");

    [Theory]
    [InlineData("")]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    public void Validate_WhenCnhNumberHasNotElevenDigits_ReturnsCnhNumberError(string cnhNumber)
        => FieldsWithError(Request(r => r.CnhNumber = cnhNumber)).Should().ContainSingle().Which.Should().Be("CnhNumber");

    [Fact]
    public void Validate_WhenCnhCategoryIsUndefined_ReturnsCategoryError()
        => FieldsWithError(Request(r => r.CnhCategory = 0)).Should().ContainSingle().Which.Should().Be("CnhCategory");

    [Fact]
    public void Validate_WhenCnhExpirationIsMissing_ReturnsExpirationError()
        => FieldsWithError(Request(r => r.CnhExpiresOn = default)).Should().ContainSingle().Which.Should().Be("CnhExpiresOn");

    [Theory]
    [InlineData("-0.01")]
    [InlineData("100.01")]
    [InlineData("12.345")]
    public void Validate_WhenCommissionIsOutOfRangeOrHasThreeDecimals_ReturnsCommissionError(string percent)
        => FieldsWithError(Request(r => r.DefaultCommissionPercent = decimal.Parse(percent, System.Globalization.CultureInfo.InvariantCulture)))
            .Should().ContainSingle().Which.Should().Be("DefaultCommissionPercent");

    [Theory]
    [InlineData("0")]
    [InlineData("100")]
    [InlineData("12.5")]
    public void Validate_WhenCommissionIsWithinRange_ReturnsValid(string percent)
        => _validator.Validate(Request(r => r.DefaultCommissionPercent = decimal.Parse(percent, System.Globalization.CultureInfo.InvariantCulture)))
            .IsValid.Should().BeTrue();

    [Fact]
    public void Validate_WhenBankDetailsAreTooLong_ReturnsBankDetailsError()
        => FieldsWithError(Request(r => r.BankDetails = new string('x', DriverProfileRules.BankDetailsMaxLength + 1)))
            .Should().ContainSingle().Which.Should().Be("BankDetails");

    private IEnumerable<string> FieldsWithError(CreateDriverRequest request)
        => _validator.Validate(request).Errors.Select(error => error.Field);

    private static CreateDriverRequest Request(Action<CreateDriverRequest> change)
    {
        CreateDriverRequest request = ValidRequest();
        change(request);
        return request;
    }

    private static CreateDriverRequest ValidRequest()
        => new()
        {
            Name = "João da Silva",
            Cpf = "529.982.247-25",
            Phone = "(11) 98765-4321",
            CnhNumber = "12345678901",
            CnhCategory = ECnhCategory.E,
            CnhExpiresOn = new DateOnly(2028, 5, 10),
            ToxicologyExamExpiresOn = new DateOnly(2027, 1, 15),
            DefaultCommissionPercent = 12.5m,
            BankDetails = "PIX: 11987654321"
        };
}
