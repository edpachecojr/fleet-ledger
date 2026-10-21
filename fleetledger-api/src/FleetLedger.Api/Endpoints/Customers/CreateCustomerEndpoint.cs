using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Customers;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Customers;

public sealed class CreateCustomerEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Customers: Create")
            .WithSummary("Cadastra um cliente")
            .AddEndpointFilter<ValidationFilter<CreateCustomerRequest>>()
            .Produces<Response<Customer?>>(StatusCodes.Status201Created)
            .Produces<Response<Customer?>>(StatusCodes.Status409Conflict)
            .ProducesValidationProblem();

    private static async Task<IResult> HandleAsync(
        IUserContext userContext,
        ICustomerHandler handler,
        CreateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        request.OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken);

        Response<Customer?> result = await handler.CreateAsync(request, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Created($"/v1/customers/{result.Data?.Id}", result)
            : TypedResults.Json(result, statusCode: result.Code);
    }
}
