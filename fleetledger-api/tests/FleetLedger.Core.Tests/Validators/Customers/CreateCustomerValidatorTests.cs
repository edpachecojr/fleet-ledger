using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Customers;
using FleetLedger.Core.Validators.Customers;

namespace FleetLedger.Core.Tests.Validators.Customers;

public class CreateCustomerValidatorTests
{
    private readonly CreateCustomerValidator _validator = new();

    [Fact]
    public void Validate_WithRequiredFieldsOnly_ReturnsValid()
        => _validator.Validate(new CreateCustomerRequest
        {
            CorporateName = "Agro Sul Ltda",
            Cnpj = "11222333000181",
            BillingCycle = EBillingCycle.Weekly
        }).IsValid.Should().BeTrue();

    [Fact]
    public void Validate_WithEveryFieldFilled_ReturnsValid()
        => _validator.Validate(ValidRequest()).IsValid.Should().BeTrue();

    [Theory]
    [InlineData("")]
    [InlineData("11.222.333/0001-82")]
    [InlineData("11111111111111")]
    public void Validate_WhenCnpjIsMissingOrInvalid_ReturnsCnpjError(string cnpj)
        => FieldsWithError(Request(r => r.Cnpj = cnpj)).Should().ContainSingle().Which.Should().Be("Cnpj");

    [Fact]
    public void Validate_WhenCnpjIsInvalid_MessageNamesTheValue()
        => _validator.Validate(Request(r => r.Cnpj = "123")).Errors.Single().Message.Should().Contain("'123'");

    [Fact]
    public void Validate_WhenCorporateNameIsBlank_ReturnsCorporateNameError()
        => FieldsWithError(Request(r => r.CorporateName = " ")).Should().ContainSingle().Which.Should().Be("CorporateName");

    [Fact]
    public void Validate_WhenTradeNameIsTooLong_ReturnsTradeNameError()
        => FieldsWithError(Request(r => r.TradeName = new string('n', CustomerProfileRules.NameMaxLength + 1)))
            .Should().ContainSingle().Which.Should().Be("TradeName");

    [Theory]
    [InlineData("98765-4321")]
    [InlineData("abc")]
    public void Validate_WhenPhoneIsInformedWithoutAreaCode_ReturnsPhoneError(string phone)
        => FieldsWithError(Request(r => r.Phone = phone)).Should().ContainSingle().Which.Should().Be("Phone");

    [Theory]
    [InlineData("financeiro")]
    [InlineData("financeiro@empresa")]
    [InlineData("a b@empresa.com")]
    [InlineData("a@@empresa.com")]
    public void Validate_WhenEmailIsMalformed_ReturnsEmailError(string email)
        => FieldsWithError(Request(r => r.Email = email)).Should().ContainSingle().Which.Should().Be("Email");

    [Fact]
    public void Validate_WhenBillingCycleIsMissing_ReturnsBillingCycleError()
        => FieldsWithError(Request(r => r.BillingCycle = 0)).Should().ContainSingle().Which.Should().Be("BillingCycle");

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Validate_WhenPaymentTermIsBelowOneDay_ReturnsPaymentTermError(int days)
        => FieldsWithError(Request(r => r.PaymentTermDays = days)).Should().ContainSingle().Which.Should().Be("PaymentTermDays");

    private IEnumerable<string> FieldsWithError(CreateCustomerRequest request)
        => _validator.Validate(request).Errors.Select(error => error.Field);

    private static CreateCustomerRequest Request(Action<CreateCustomerRequest> change)
    {
        CreateCustomerRequest request = ValidRequest();
        change(request);
        return request;
    }

    private static CreateCustomerRequest ValidRequest()
        => new()
        {
            CorporateName = "Agro Sul Ltda",
            TradeName = "Agro Sul",
            Cnpj = "11.222.333/0001-81",
            Phone = "(11) 98765-4321",
            Email = "financeiro@agrosul.com.br",
            BillingCycle = EBillingCycle.Monthly,
            PaymentTermDays = 30
        };
}
