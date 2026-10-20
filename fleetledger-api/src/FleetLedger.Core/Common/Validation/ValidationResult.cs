namespace FleetLedger.Core.Common.Validation;

/// <summary>
/// Resultado de uma validação: válido quando não há nenhum erro.
/// </summary>
/// <param name="errors">Erros encontrados; vazio significa válido.</param>
public sealed class ValidationResult(IReadOnlyList<ValidationError> errors)
{
    /// <summary>Resultado sem erros, compartilhado para evitar alocação.</summary>
    public static readonly ValidationResult Valid = new([]);

    /// <summary>Erros encontrados, na ordem em que as regras foram avaliadas.</summary>
    public IReadOnlyList<ValidationError> Errors { get; } = errors;

    /// <summary>Verdadeiro quando nenhuma regra foi violada.</summary>
    public bool IsValid => Errors.Count == 0;

    /// <summary>
    /// Agrupa as mensagens por campo, no formato de <c>TypedResults.ValidationProblem</c>.
    /// </summary>
    /// <example><code>TypedResults.ValidationProblem(result.ToDictionary())</code></example>
    public Dictionary<string, string[]> ToDictionary()
        => Errors
            .GroupBy(error => error.Field)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.Message).ToArray());
}
