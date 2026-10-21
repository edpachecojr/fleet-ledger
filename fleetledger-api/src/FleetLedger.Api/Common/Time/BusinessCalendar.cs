using FleetLedger.Api.Options;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Common.Time;

// O relógio vem do TimeProvider injetado; o fuso, da configuração validada no boot.
public sealed class BusinessCalendar(TimeProvider timeProvider, IOptions<CalendarOptions> options) : IBusinessCalendar
{
    private readonly TimeZoneInfo _timeZone = TimeZoneInfo.FindSystemTimeZoneById(options.Value.TimeZone);

    public DateOnly Today()
        => DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(timeProvider.GetUtcNow(), _timeZone).DateTime);
}
