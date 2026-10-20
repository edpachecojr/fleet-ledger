using FleetLedger.Core.Common.Validation;

namespace FleetLedger.Core.Tests.Common.Validation;

public class ValidationBuilderTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Required_WhenValueIsBlank_AddsError(string? value)
    {
        ValidationResult result = new ValidationBuilder().Required("Name", value, "Nome é obrigatório").Build();

        result.Errors.Should().ContainSingle(error => error.Field == "Name" && error.Message == "Nome é obrigatório");
    }

    [Fact]
    public void Required_WhenValueIsFilled_IsValid()
        => new ValidationBuilder().Required("Name", "FleetLedger", "erro").Build().IsValid.Should().BeTrue();

    [Theory]
    [InlineData("abcd", false)]
    [InlineData("abc", true)]
    [InlineData(null, true)]
    public void MaxLength_ForValue_ValidatesLimit(string? value, bool expectedValid)
        => new ValidationBuilder().MaxLength("Name", value, 3, "erro").Build().IsValid.Should().Be(expectedValid);

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(0.01, true)]
    public void GreaterThan_ForValue_RequiresStrictlyGreater(decimal value, bool expectedValid)
        => new ValidationBuilder().GreaterThan("Amount", value, 0, "erro").Build().IsValid.Should().Be(expectedValid);

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(10, true)]
    [InlineData(11, false)]
    public void Range_ForValue_AcceptsClosedInterval(int value, bool expectedValid)
        => new ValidationBuilder().Range("PageSize", value, 1, 10, "erro").Build().IsValid.Should().Be(expectedValid);

    [Fact]
    public void Build_WhenNoRuleFails_ReturnsSharedValidResult()
        => new ValidationBuilder().When(false, "Field", "erro").Build().Should().BeSameAs(ValidationResult.Valid);

    [Fact]
    public void ToDictionary_WhenFieldHasManyErrors_GroupsMessagesByField()
    {
        ValidationResult result = new ValidationBuilder()
            .When(true, "Name", "primeira")
            .When(true, "Name", "segunda")
            .When(true, "Plate", "terceira")
            .Build();

        Dictionary<string, string[]> errors = result.ToDictionary();

        errors["Name"].Should().Equal("primeira", "segunda");
        errors["Plate"].Should().Equal("terceira");
    }
}
