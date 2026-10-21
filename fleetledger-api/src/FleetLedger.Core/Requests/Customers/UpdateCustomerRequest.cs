using System.Text.Json.Serialization;
using FleetLedger.Core.Models;

namespace FleetLedger.Core.Requests.Customers;

/// <summary>Edição do cliente: tudo menos o CNPJ, que é a identidade do cadastro.</summary>
/// <example><code>new UpdateCustomerRequest { CorporateName = "Agro Sul Ltda", BillingCycle = EBillingCycle.Biweekly }</code></example>
public sealed class UpdateCustomerRequest : Request
{
    /// <summary>Id do cliente, lido da rota.</summary>
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <summary>Razão social (obrigatória).</summary>
    public string CorporateName { get; set; } = string.Empty;

    /// <summary>Nome fantasia (opcional).</summary>
    public string? TradeName { get; set; }

    /// <summary>Telefone com DDD (opcional).</summary>
    public string? Phone { get; set; }

    /// <summary>E-mail de contato (opcional).</summary>
    public string? Email { get; set; }

    /// <summary>Ciclo de faturamento (obrigatório).</summary>
    public EBillingCycle BillingCycle { get; set; }

    /// <summary>Prazo de pagamento em dias (opcional, 1 ou mais).</summary>
    public int? PaymentTermDays { get; set; }
}
