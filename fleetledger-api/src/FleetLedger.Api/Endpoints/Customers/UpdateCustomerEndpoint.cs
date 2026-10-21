using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Models;
using FleetLedger.Core.Requests.Customers;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Security;

namespace FleetLedger.Api.Endpoints.Customers;

public sealed class UpdateCustomerEndpoint : IEndpoint
{
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/{id:guid}", HandleAsync)
            .WithName("Customers: Update")
            .WithSummary("Altera os dados de um cliente")
            .WithDescription("O CNPJ não é editável.")
            .AddEndpointFilter<ValidationFilter<UpdateCustomerRequest>>()
            .Produces<Response<Customer?>>(StatusCodes.Status200OK)
            .Produces<Response<Customer?>>(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();

    private static async Task<IResult> HandleAsync(
        Guid id,
        IUserContext userContext,
        ICustomerHandler handler,
        UpdateCustomerRequest request,
        CancellationToken cancellationToken)
    {
        request.Id = id;
        request.OrganizationId = await userContext.GetOrganizationIdAsync(cancellationToken);

        Response<Customer?> result = await handler.UpdateAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
