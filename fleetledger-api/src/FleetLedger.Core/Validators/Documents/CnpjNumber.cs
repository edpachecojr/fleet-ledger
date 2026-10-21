namespace FleetLedger.Core.Validators.Documents;

/// <summary>CNPJ com dígitos verificadores (módulo 11), guardado só com os 14 dígitos.</summary>
/// <example><code>CnpjNumber.IsValid(DigitsOnly.Of("11.222.333/0001-81")) // true</code></example>
public static class CnpjNumber
{
    /// <summary>Quantidade de dígitos do CNPJ.</summary>
    public const int Length = 14;

    private static readonly int[] FirstCheckWeights = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
    private static readonly int[] SecondCheckWeights = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

    /// <summary>
    /// Verdadeiro para 14 dígitos com os verificadores corretos. Repetições ("11111111111111")
    /// passam no cálculo, mas não são CNPJ emitido e são recusadas.
    /// </summary>
    public static bool IsValid(string digits)
        => digits.Length == Length
            && digits.All(char.IsAsciiDigit)
            && digits.Distinct().Count() > 1
            && CheckDigit(digits, FirstCheckWeights) == digits[12] - '0'
            && CheckDigit(digits, SecondCheckWeights) == digits[13] - '0';

    private static int CheckDigit(string digits, int[] weights)
    {
        int remainder = weights.Select((weight, index) => (digits[index] - '0') * weight).Sum() % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
