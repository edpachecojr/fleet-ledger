using FleetLedger.Api.Common.Api;
using Microsoft.AspNetCore.Http;

namespace FleetLedger.Api.Tests.Common.Api;

public class HttpContractExtensionTests
{
    // Regressão: GET /v1/ledger-accounts/selectable sem entryContext respondia 500.
    [Fact]
    public void SelectStatusCode_ForParameterBindingFailure_ReturnsItsClientErrorStatus()
        => HttpContractExtension.SelectStatusCode(new BadHttpRequestException("Required parameter missing"))
            .Should().Be(StatusCodes.Status400BadRequest);

    [Fact]
    public void SelectStatusCode_ForUnexpectedException_Returns500()
        => HttpContractExtension.SelectStatusCode(new InvalidOperationException("boom"))
            .Should().Be(StatusCodes.Status500InternalServerError);
}
