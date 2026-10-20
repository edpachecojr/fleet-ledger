using FleetLedger.Api.Common.Api;
using FleetLedger.Api.Tests.Fakes;
using FleetLedger.Core.Common.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FleetLedger.Api.Tests.Common.Api;

public class ValidationFilterTests
{
    private const string NextResult = "next";

    [Fact]
    public async Task InvokeAsync_WhenRequestIsValid_CallsNext()
    {
        ValidationFilter<SampleRequest> filter = new(new FakeValidator<SampleRequest>(ValidationResult.Valid));

        object? result = await filter.InvokeAsync(ContextWith(new SampleRequest()), CallNext);

        result.Should().Be(NextResult);
    }

    [Fact]
    public async Task InvokeAsync_WhenRequestIsInvalid_ReturnsValidationProblemByField()
    {
        ValidationResult invalid = new([new ValidationError("Name", "Nome é obrigatório")]);
        ValidationFilter<SampleRequest> filter = new(new FakeValidator<SampleRequest>(invalid));

        object? result = await filter.InvokeAsync(ContextWith(new SampleRequest()), CallNext);

        result.Should().BeOfType<ValidationProblem>()
            .Which.ProblemDetails.Errors.Should().ContainKey("Name");
    }

    [Fact]
    public async Task InvokeAsync_WhenBodyIsMissing_ReturnsBadRequestProblem()
    {
        ValidationFilter<SampleRequest> filter = new(new FakeValidator<SampleRequest>(ValidationResult.Valid));

        object? result = await filter.InvokeAsync(ContextWith(), CallNext);

        result.Should().BeOfType<ProblemHttpResult>()
            .Which.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
    }

    private static DefaultEndpointFilterInvocationContext ContextWith(params object[] arguments)
        => new(new DefaultHttpContext(), arguments);

    private static ValueTask<object?> CallNext(EndpointFilterInvocationContext context)
        => ValueTask.FromResult<object?>(NextResult);
}
