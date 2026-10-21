using FleetLedger.Api.Common.Api;
using FleetLedger.Core.Handlers;
using FleetLedger.Core.Requests.Auth;
using FleetLedger.Core.Responses;
using FleetLedger.Core.Responses.Auth;

namespace FleetLedger.Api.Endpoints.Auth;

public sealed class LoginEndpoint : IEndpoint
{
    // Anônima por definição: é aqui que o cliente obtém o access token.
    public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/login", HandleAsync)
            .WithName("Auth: Login")
            .WithSummary("Autentica o administrador e emite o access token")
            .WithDescription("Responde 401 com mensagem genérica para qualquer credencial recusada.")
            .AllowAnonymous()
            .AddEndpointFilter<ValidationFilter<LoginRequest>>()
            .Produces<Response<IssuedAccessToken?>>(StatusCodes.Status200OK)
            .Produces<Response<IssuedAccessToken?>>(StatusCodes.Status401Unauthorized)
            .ProducesValidationProblem();

    private static async Task<IResult> HandleAsync(
        IAuthHandler handler,
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        Response<IssuedAccessToken?> result = await handler.LoginAsync(request, cancellationToken);

        return TypedResults.Json(result, statusCode: result.Code);
    }
}
