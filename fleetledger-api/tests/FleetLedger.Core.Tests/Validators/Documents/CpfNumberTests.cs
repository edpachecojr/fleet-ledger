using FleetLedger.Core.Validators.Documents;

namespace FleetLedger.Core.Tests.Validators.Documents;

public class CpfNumberTests
{
    [Theory]
    [InlineData("52998224725")]
    [InlineData("11144477735")]
    public void IsValid_WithCorrectCheckDigits_ReturnsTrue(string cpf) => CpfNumber.IsValid(cpf).Should().BeTrue();

    [Theory]
    [InlineData("52998224726")]
    [InlineData("52998224715")]
    [InlineData("5299822472")]
    [InlineData("11111111111")]
    [InlineData("")]
    public void IsValid_WithWrongDigitsLengthOrRepetition_ReturnsFalse(string cpf) => CpfNumber.IsValid(cpf).Should().BeFalse();
}
