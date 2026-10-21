namespace FleetLedger.Api.Options;

// O MVP não tem cadastro de usuários (TASK-002): o único administrador e a sua
// organização nascem desta configuração na primeira subida com o banco vazio.
public sealed class AdministratorSeedOptions
{
    public const string SectionName = "AdministratorSeed";

    // A política de senha do Identity ainda exige maiúscula, minúscula, dígito e símbolo.
    public const int MinimumPasswordLength = 12;

    public string OrganizationName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}
