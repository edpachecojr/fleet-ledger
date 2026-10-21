namespace FleetLedger.Core.Validators.Documents;

/// <summary>CPF com dígitos verificadores (módulo 11), guardado só com os 11 dígitos.</summary>
/// <example><code>CpfNumber.IsValid(DigitsOnly.Of("529.982.247-25")) // true</code></example>
public static class CpfNumber
{
    /// <summary>Quantidade de dígitos do CPF.</summary>
    public const int Length = 11;

    /// <summary>
    /// Verdadeiro para 11 dígitos com os verificadores corretos. Repetições ("11111111111")
    /// passam no cálculo, mas não são CPF emitido e são recusadas.
    /// </summary>
    public static bool IsValid(string digits)
        => digits.Length == Length
            && digits.All(char.IsAsciiDigit)
            && digits.Distinct().Count() > 1
            && CheckDigit(digits, 9) == digits[9] - '0'
            && CheckDigit(digits, 10) == digits[10] - '0';

    // Pesos decrescentes a partir de (quantidade + 1): 10..2 para o 1º dígito, 11..2 para o 2º.
    private static int CheckDigit(string digits, int count)
    {
        int sum = Enumerable.Range(0, count).Sum(index => (digits[index] - '0') * (count + 1 - index));
        int remainder = sum % 11;
        return remainder < 2 ? 0 : 11 - remainder;
    }
}
