using FleetLedger.Core.Common.Validation;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Options;

public sealed class DatabaseOptionsValidator : IValidateOptions<DatabaseOptions>
{
    public ValidateOptionsResult Validate(string? name, DatabaseOptions options)
        => new ValidationBuilder()
            .Required(
                $"{DatabaseOptions.SectionName}:ConnectionString",
                options.ConnectionString,
                $"{DatabaseOptions.SectionName}:ConnectionString é obrigatório. " +
                "Esperado: string de conexão PostgreSQL, ex.: Host=localhost;Database=fleetledger;Username=fleetledger;Password=...")
            .Build()
            .ToOptionsResult();
}
