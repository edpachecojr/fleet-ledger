using FleetLedger.Core.Models;

namespace FleetLedger.Core.Requests.Drivers;

/// <summary>
/// Campos editáveis do motorista, comuns ao cadastro e à edição (o CPF fica de fora: é
/// identidade e só existe no cadastro).
/// </summary>
/// <example><code>public sealed class UpdateDriverRequest : DriverProfileFields</code></example>
public abstract class DriverProfileFields : Request
{
    /// <summary>Nome completo (obrigatório).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Telefone com DDD (obrigatório).</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>Número da CNH com 11 dígitos, único na organização.</summary>
    public string CnhNumber { get; set; } = string.Empty;

    /// <summary>Categoria da CNH (obrigatória).</summary>
    public ECnhCategory CnhCategory { get; set; }

    /// <summary>Validade da CNH (obrigatória).</summary>
    public DateOnly CnhExpiresOn { get; set; }

    /// <summary>Validade do exame toxicológico (opcional).</summary>
    public DateOnly? ToxicologyExamExpiresOn { get; set; }

    /// <summary>Comissão padrão em %, de 0 a 100, com até 2 casas.</summary>
    public decimal DefaultCommissionPercent { get; set; }

    /// <summary>Dados bancários ou chave PIX (opcional).</summary>
    public string? BankDetails { get; set; }
}
