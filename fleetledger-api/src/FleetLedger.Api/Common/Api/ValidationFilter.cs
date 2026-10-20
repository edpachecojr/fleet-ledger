using FleetLedger.Core.Common.Validation;

namespace FleetLedger.Api.Common.Api;

// Minimal API não valida nada sozinha: todo endpoint que recebe corpo declara este filtro.
public sealed class ValidationFilter<TRequest>(IValidator<TRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        TRequest? request = context.Arguments.OfType<TRequest>().FirstOrDefault();
        if (request is null)
            return TypedResults.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                detail: $"Corpo da requisição ausente. Esperado: {typeof(TRequest).Name} em JSON.");

        ValidationResult result = validator.Validate(request);
        if (!result.IsValid)
            return TypedResults.ValidationProblem(result.ToDictionary());

        return await next(context);
    }
}
