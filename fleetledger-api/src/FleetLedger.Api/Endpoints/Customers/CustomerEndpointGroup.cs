using FleetLedger.Api.Common.Api;

namespace FleetLedger.Api.Endpoints.Customers;

// Sem DELETE: cliente com histórico só é inativado (TASK-006).
public static class CustomerEndpointGroup
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        => app.MapGroup("v1/customers")
            .WithTags("Customers")
            .MapEndpoint<CreateCustomerEndpoint>()
            .MapEndpoint<ListCustomersEndpoint>()
            .MapEndpoint<GetCustomerEndpoint>()
            .MapEndpoint<UpdateCustomerEndpoint>()
            .MapEndpoint<DeactivateCustomerEndpoint>()
            .MapEndpoint<ReactivateCustomerEndpoint>();
}
