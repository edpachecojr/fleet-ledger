namespace FleetLedger.Core.Common.Validation;

/// <summary>
/// Base de todo validador: a subclasse declara as regras, a base monta o resultado.
/// </summary>
/// <example>
/// <code>
/// public sealed class CreateVehicleValidator : Validator&lt;CreateVehicleRequest&gt;
/// {
///     protected override void Rules(CreateVehicleRequest request, ValidationBuilder rules)
///         => rules.Required(nameof(request.Plate), request.Plate, "Placa é obrigatória");
/// }
/// </code>
/// </example>
/// <typeparam name="TRequest">Requisição validada.</typeparam>
public abstract class Validator<TRequest> : IValidator<TRequest>
{
    /// <summary>Declara as regras da requisição sobre o <paramref name="rules"/>.</summary>
    protected abstract void Rules(TRequest request, ValidationBuilder rules);

    /// <inheritdoc />
    public ValidationResult Validate(TRequest request)
    {
        ValidationBuilder rules = new();
        Rules(request, rules);
        return rules.Build();
    }
}
