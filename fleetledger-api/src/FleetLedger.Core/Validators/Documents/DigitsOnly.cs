namespace FleetLedger.Core.Validators.Documents;

/// <summary>Remove máscara de documento e telefone: "11.222.333/0001-81" vira "11222333000181".</summary>
/// <example><code>DigitsOnly.Of("(11) 98765-4321") // "11987654321"</code></example>
public static class DigitsOnly
{
    /// <summary>Só os dígitos de <paramref name="value"/>; nulo vira vazio.</summary>
    public static string Of(string? value) => new((value ?? string.Empty).Where(char.IsAsciiDigit).ToArray());
}
