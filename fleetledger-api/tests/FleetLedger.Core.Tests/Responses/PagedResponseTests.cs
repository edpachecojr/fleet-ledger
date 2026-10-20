using FleetLedger.Core.Responses;

namespace FleetLedger.Core.Tests.Responses;

public class PagedResponseTests
{
    [Theory]
    [InlineData(0, 25, 0)]
    [InlineData(25, 25, 1)]
    [InlineData(26, 25, 2)]
    [InlineData(10, 0, 0)]
    public void TotalPages_ForTotalCountAndPageSize_RoundsUp(int totalCount, int pageSize, int expected)
    {
        PagedResponse<int[]> response = new([], totalCount, currentPage: 1, pageSize: pageSize);

        response.TotalPages.Should().Be(expected);
    }

    [Fact]
    public void Constructor_WithPageData_ReturnsSuccess()
    {
        PagedResponse<int[]> response = new([1, 2], totalCount: 2);

        response.IsSuccess.Should().BeTrue();
        response.CurrentPage.Should().Be(1);
    }
}
