namespace FleetLedger.Core.Common.Validation;

/// <summary>
/// Valida uma requisição de entrada. Implementações não têm estado e são singletons.
/// </summary>
/// <typeparam name="TRequest">Requisição validada.</typeparam>
public interface IValidator<in TRequest>
{
    /// <summary>Avalia todas as regras e devolve os erros encontrados.</summary>
    /// <example><code>ValidationResult result = validator.Validate(request);</code></example>
    ValidationResult Validate(TRequest request);
}
