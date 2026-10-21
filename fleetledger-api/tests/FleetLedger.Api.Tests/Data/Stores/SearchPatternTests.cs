using FleetLedger.Api.Data.Stores;

namespace FleetLedger.Api.Tests.Data.Stores;

public class SearchPatternTests
{
    [Theory]
    [InlineData("volvo", "%volvo%")]
    [InlineData("10%", @"%10\%%")]
    [InlineData("a_b", @"%a\_b%")]
    [InlineData(@"c:\x", @"%c:\\x%")]
    public void Contains_EscapesWildcardsTypedByTheUser(string term, string expected)
        => SearchPattern.Contains(term).Should().Be(expected);
}
