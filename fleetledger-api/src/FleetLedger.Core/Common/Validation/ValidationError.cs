namespace FleetLedger.Core.Common.Validation;

/// <summary>
/// Uma regra violada: o campo ofendido e a mensagem que diz o que se esperava.
/// </summary>
/// <example><code>new ValidationError("Name", "Nome é obrigatório")</code></example>
/// <param name="Field">Nome do campo como o cliente o conhece.</param>
/// <param name="Message">Mensagem para o usuário, com o formato esperado.</param>
public sealed record ValidationError(string Field, string Message);
