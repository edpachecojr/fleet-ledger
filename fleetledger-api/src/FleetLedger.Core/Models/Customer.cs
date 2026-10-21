namespace FleetLedger.Core.Models;

/// <summary>
/// Cliente que contrata fretes e recebe as faturas. O CNPJ é a identidade do cadastro e não
/// muda depois de criado; com histórico, o cliente só é inativado.
/// </summary>
/// <example><code>new Customer { CorporateName = "Agro Sul Ltda", Cnpj = "11222333000181", BillingCycle = EBillingCycle.Monthly }</code></example>
public sealed class Customer : BaseModel, IOrganizationOwned, IDeactivatable
{
    /// <inheritdoc />
    public Guid OrganizationId { get; set; }

    /// <summary>Razão social.</summary>
    public string CorporateName { get; set; } = string.Empty;

    /// <summary>Nome fantasia, opcional.</summary>
    public string? TradeName { get; set; }

    /// <summary>CNPJ só com os 14 dígitos. Imutável.</summary>
    public string Cnpj { get; init; } = string.Empty;

    /// <summary>Telefone de contato só com dígitos (DDD + número), opcional.</summary>
    public string? Phone { get; set; }

    /// <summary>E-mail de contato, opcional.</summary>
    public string? Email { get; set; }

    /// <summary>Ciclo de faturamento.</summary>
    public EBillingCycle BillingCycle { get; set; }

    /// <summary>Prazo de pagamento em dias, opcional; sugere o vencimento da fatura.</summary>
    public int? PaymentTermDays { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? DeactivatedAt { get; set; }

    /// <summary>Cliente ativo: aparece na abertura de viagem.</summary>
    public bool IsActive => DeactivatedAt is null;
}
