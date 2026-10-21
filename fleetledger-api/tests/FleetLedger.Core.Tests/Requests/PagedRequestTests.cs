using FleetLedger.Core.Common.Constants;
using FleetLedger.Core.Requests.Vehicles;

namespace FleetLedger.Core.Tests.Requests;

public class PagedRequestTests
{
    [Theory]
    [InlineData(0, 1)]
    [InlineData(-5, 1)]
    [InlineData(3, 3)]
    public void EffectivePageNumber_NeverGoesBelowOne(int pageNumber, int expected)
        => new ListVehiclesRequest { PageNumber = pageNumber }.EffectivePageNumber.Should().Be(expected);

    [Theory]
    [InlineData(0, 1)]
    [InlineData(10, 10)]
    [InlineData(10_000, PagingDefaults.MaxPageSize)]
    public void EffectivePageSize_StaysBetweenOneAndTheServerMaximum(int pageSize, int expected)
        => new ListVehiclesRequest { PageSize = pageSize }.EffectivePageSize.Should().Be(expected);
}
