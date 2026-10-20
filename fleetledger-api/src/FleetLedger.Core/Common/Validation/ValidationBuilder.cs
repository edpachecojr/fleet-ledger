namespace FleetLedger.Core.Common.Validation;

/// <summary>
/// Predicados reutilizáveis de validação. Acrescente um método só quando a mesma
/// verificação aparecer pela terceira vez.
/// </summary>
/// <example>
/// <code>
/// ValidationResult result = new ValidationBuilder()
///     .Required("Name", request.Name, "Nome é obrigatório")
///     .MaxLength("Name", request.Name, 120, "Nome deve ter até 120 caracteres")
///     .Build();
/// </code>
/// </example>
public sealed class ValidationBuilder
{
    private readonly List<ValidationError> _errors = [];

    /// <summary>Registra o erro quando <paramref name="isInvalid"/> é verdadeiro.</summary>
    public ValidationBuilder When(bool isInvalid, string field, string message)
    {
        if (isInvalid)
            _errors.Add(new ValidationError(field, message));

        return this;
    }

    /// <summary>Exige texto não nulo, não vazio e não composto só de espaços.</summary>
    public ValidationBuilder Required(string field, string? value, string message)
        => When(string.IsNullOrWhiteSpace(value), field, message);

    /// <summary>Limita o comprimento do texto; nulo é aceito (use <see cref="Required"/> para exigir).</summary>
    public ValidationBuilder MaxLength(string field, string? value, int max, string message)
        => When(value is not null && value.Length > max, field, message);

    /// <summary>Exige valor estritamente maior que <paramref name="floor"/>.</summary>
    public ValidationBuilder GreaterThan(string field, decimal value, decimal floor, string message)
        => When(value <= floor, field, message);

    /// <summary>Exige valor dentro do intervalo fechado [<paramref name="min"/>, <paramref name="max"/>].</summary>
    public ValidationBuilder Range(string field, int value, int min, int max, string message)
        => When(value < min || value > max, field, message);

    /// <summary>Encerra a validação e devolve o resultado.</summary>
    public ValidationResult Build()
        => _errors.Count == 0 ? ValidationResult.Valid : new ValidationResult([.. _errors]);
}
