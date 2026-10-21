using FleetLedger.Core.Validators.Documents;

namespace FleetLedger.Core.Tests.Validators.Documents;

public class CnpjNumberTests
{
    [Theory]
    [InlineData("11222333000181")]
    [InlineData("11444777000161")]
    public void IsValid_WithCorrectCheckDigits_ReturnsTrue(string cnpj) => CnpjNumber.IsValid(cnpj).Should().BeTrue();

    [Theory]
    [InlineData("11222333000182")]
    [InlineData("11222333000171")]
    [InlineData("1122233300018")]
    [InlineData("00000000000000")]
    [InlineData("11111111111111")]
    [InlineData("")]
    public void IsValid_WithWrongDigitsLengthOrRepetition_ReturnsFalse(string cnpj) => CnpjNumber.IsValid(cnpj).Should().BeFalse();
}
