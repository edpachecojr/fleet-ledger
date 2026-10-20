using FleetLedger.Api.Options;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Tests.Options;

public class DatabaseOptionsValidatorTests
{
    private readonly DatabaseOptionsValidator _validator = new();

    [Fact]
    public void Validate_WhenConnectionStringIsSet_Succeeds()
    {
        ValidateOptionsResult result = _validator.Validate(null, new DatabaseOptions { ConnectionString = "Host=db" });

        result.Succeeded.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    public void Validate_WhenConnectionStringIsBlank_FailsNamingTheKey(string connectionString)
    {
        ValidateOptionsResult result = _validator.Validate(null, new DatabaseOptions { ConnectionString = connectionString });

        result.Failed.Should().BeTrue();
        result.FailureMessage.Should().Contain("Database:ConnectionString");
    }
}
