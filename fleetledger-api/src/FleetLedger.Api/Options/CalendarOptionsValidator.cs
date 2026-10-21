using FleetLedger.Core.Common.Validation;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Options;

public sealed class CalendarOptionsValidator : IValidateOptions<CalendarOptions>
{
    private const string TimeZoneKey = $"{CalendarOptions.SectionName}:TimeZone";

    public ValidateOptionsResult Validate(string? name, CalendarOptions options)
        => new ValidationBuilder()
            .When(!TimeZoneInfo.TryFindSystemTimeZoneById(options.TimeZone, out _), TimeZoneKey,
                $"{TimeZoneKey} '{options.TimeZone}' não é um fuso conhecido. Esperado: id IANA, ex.: America/Sao_Paulo")
            .Build()
            .ToOptionsResult();
}
