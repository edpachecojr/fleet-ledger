using FleetLedger.Core.Models;

namespace FleetLedger.Core.Requests.Customers;

/// <summary>Cadastro de cliente. CNPJ e telefone aceitam máscara; são guardados só com dígitos.</summary>
/// <example><code>new CreateCustomerRequest { CorporateName = "Agro Sul Ltda", Cnpj = "11.222.333/0001-81", BillingCycle = EBillingCycle.Monthly }</code></example>
public sealed class CreateCustomerRequest : Request
{
    /// <summary>Razão social (obrigatória).</summary>
    public string CorporateName { get; set; } = string.Empty;

    /// <summary>Nome fantasia (opcional).</summary>
    public string? TradeName { get; set; }

    /// <summary>CNPJ com dígitos verificadores válidos, único na organização.</summary>
    public string Cnpj { get; set; } = string.Empty;

    /// <summary>Telefone com DDD (opcional).</summary>
    public string? Phone { get; set; }

    /// <summary>E-mail de contato (opcional).</summary>
    public string? Email { get; set; }

    /// <summary>Ciclo de faturamento (obrigatório).</summary>
    public EBillingCycle BillingCycle { get; set; }

    /// <summary>Prazo de pagamento em dias (opcional, 1 ou mais).</summary>
    public int? PaymentTermDays { get; set; }
}
