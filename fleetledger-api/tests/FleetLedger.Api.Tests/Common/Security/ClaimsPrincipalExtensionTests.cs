using System.Security.Claims;
using FleetLedger.Api.Common.Security;

namespace FleetLedger.Api.Tests.Common.Security;

public class ClaimsPrincipalExtensionTests
{
    [Fact]
    public void GetUserId_WhenClaimHoldsGuid_ReturnsIt()
    {
        Guid userId = Guid.CreateVersion7();

        PrincipalWith(new Claim(ClaimTypes.NameIdentifier, userId.ToString())).GetUserId().Should().Be(userId);
    }

    [Fact]
    public void GetUserId_WhenClaimIsMissing_Throws()
    {
        Action act = () => PrincipalWith().GetUserId();

        act.Should().Throw<UnauthorizedAccessException>().WithMessage($"*{ClaimTypes.NameIdentifier}*ausente*");
    }

    [Theory]
    [InlineData("admin@fleetledger.example")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void GetUserId_WhenClaimIsNotAValidGuid_ThrowsWithReceivedValue(string claimValue)
    {
        Action act = () => PrincipalWith(new Claim(ClaimTypes.NameIdentifier, claimValue)).GetUserId();

        act.Should().Throw<UnauthorizedAccessException>().WithMessage($"*'{claimValue}'*");
    }

    private static ClaimsPrincipal PrincipalWith(params Claim[] claims) => new(new ClaimsIdentity(claims, "Test"));
}
