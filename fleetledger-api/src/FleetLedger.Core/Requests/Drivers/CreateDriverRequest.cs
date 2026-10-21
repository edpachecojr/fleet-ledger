namespace FleetLedger.Core.Requests.Drivers;

/// <summary>Cadastro de motorista. CPF, telefone e CNH aceitam máscara; são guardados só com dígitos.</summary>
/// <example><code>new CreateDriverRequest { Name = "João da Silva", Cpf = "529.982.247-25", CnhCategory = ECnhCategory.E }</code></example>
public sealed class CreateDriverRequest : DriverProfileFields
{
    /// <summary>CPF com dígitos verificadores válidos, único na organização.</summary>
    public string Cpf { get; set; } = string.Empty;
}
