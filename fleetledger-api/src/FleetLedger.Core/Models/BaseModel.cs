namespace FleetLedger.Core.Models;

/// <summary>
/// Base de toda entidade persistida: identidade gerada pela aplicação e datas de auditoria em UTC.
/// Duas instâncias são iguais quando têm o mesmo tipo e o mesmo <see cref="Id"/>.
/// </summary>
/// <example>
/// <code>
/// var organization = new Organization { Name = "Transportes Exemplo", CreatedAt = now, UpdatedAt = now };
/// </code>
/// </example>
public abstract class BaseModel : IEquatable<BaseModel>
{
    /// <summary>
    /// UUID v7, gerado na aplicação e nunca pelo banco: a ordem temporal mantém o índice
    /// da chave primária compacto e o id existe antes do primeiro <c>SaveChanges</c>.
    /// </summary>
    public Guid Id { get; init; } = Guid.CreateVersion7();

    /// <summary>Momento da criação, em UTC. Atribuído explicitamente por quem cria a entidade.</summary>
    public DateTimeOffset CreatedAt { get; init; }

    /// <summary>Momento da última alteração, em UTC. Atribuído explicitamente por quem altera a entidade.</summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <inheritdoc />
    public bool Equals(BaseModel? other)
        => other is not null && other.GetType() == GetType() && other.Id == Id;

    /// <inheritdoc />
    public override bool Equals(object? obj) => Equals(obj as BaseModel);

    /// <inheritdoc />
    public override int GetHashCode() => Id.GetHashCode();
}
