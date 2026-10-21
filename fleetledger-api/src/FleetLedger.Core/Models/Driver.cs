namespace FleetLedger.Core.Models;

/// <summary>
/// Motorista da frota, com comissão padrão e vencimento de documentos. O CPF é a identidade
/// do cadastro e não muda depois de criado; com histórico, o motorista só é inativado.
/// </summary>
/// <example><code>new Driver { Name = "João da Silva", Cpf = "52998224725", CnhCategory = ECnhCategory.E }</code></example>
public sealed class Driver : BaseModel, IOrganizationOwned, IDeactivatable
{
    /// <inheritdoc />
    public Guid OrganizationId { get; set; }

    /// <summary>Nome completo.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>CPF só com os 11 dígitos. Imutável.</summary>
    public string Cpf { get; init; } = string.Empty;

    /// <summary>Telefone só com dígitos (DDD + número).</summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>Número da CNH (11 dígitos), único na organização.</summary>
    public string CnhNumber { get; set; } = string.Empty;

    /// <summary>Categoria da CNH.</summary>
    public ECnhCategory CnhCategory { get; set; }

    /// <summary>Validade da CNH.</summary>
    public DateOnly CnhExpiresOn { get; set; }

    /// <summary>Validade do exame toxicológico; opcional (em branco não conta na situação).</summary>
    public DateOnly? ToxicologyExamExpiresOn { get; set; }

    /// <summary>
    /// Comissão padrão em % (0 a 100, 2 casas). Alterar não afeta viagens já abertas: a viagem
    /// copia o percentual na abertura.
    /// </summary>
    public decimal DefaultCommissionPercent { get; set; }

    /// <summary>Dados bancários ou chave PIX, em texto livre, para pagar o acerto (opcional).</summary>
    public string? BankDetails { get; set; }

    /// <inheritdoc />
    public DateTimeOffset? DeactivatedAt { get; set; }

    /// <summary>Motorista ativo: aparece na abertura de viagem.</summary>
    public bool IsActive => DeactivatedAt is null;

    /// <summary>
    /// Situação documental na data de hoje, calculada pelo servidor a cada resposta; não é
    /// persistida e não pode ser enviada pelo cliente.
    /// </summary>
    public EDocumentStatus DocumentStatus { get; private set; } = EDocumentStatus.Regular;

    /// <summary>Atribui a situação calculada para a data de hoje.</summary>
    public void SetDocumentStatus(EDocumentStatus status) => DocumentStatus = status;
}
