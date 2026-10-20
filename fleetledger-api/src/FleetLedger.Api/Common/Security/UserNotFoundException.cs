namespace FleetLedger.Api.Common.Security;

public sealed class UserNotFoundException(Guid userId)
    : Exception(
        $"Usuário '{userId}' do access token não existe no banco. " +
        "Esperado: usuário cadastrado e vinculado a uma organização.")
{
    public Guid UserId { get; } = userId;
}
