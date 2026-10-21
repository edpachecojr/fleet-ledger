using FleetLedger.Api.Common.Time;
using FleetLedger.Api.Options;
using FleetLedger.Api.Tests.Fakes;
using OptionsFactory = Microsoft.Extensions.Options.Options;

namespace FleetLedger.Api.Tests.Common.Time;

public class BusinessCalendarTests
{
    // 02:00 UTC de 21/10 ainda é 23:00 de 20/10 em São Paulo (UTC-3).
    private static readonly DateTimeOffset JustAfterUtcMidnight = new(2026, 10, 21, 2, 0, 0, TimeSpan.Zero);

    [Fact]
    public void Today_UsesTheConfiguredTimeZoneInsteadOfUtc()
        => Calendar("America/Sao_Paulo").Today().Should().Be(new DateOnly(2026, 10, 20));

    [Fact]
    public void Today_InUtc_IsTheUtcDate()
        => Calendar("UTC").Today().Should().Be(new DateOnly(2026, 10, 21));

    private static BusinessCalendar Calendar(string timeZone)
        => new(new FakeTimeProvider(JustAfterUtcMidnight), OptionsFactory.Create(new CalendarOptions { TimeZone = timeZone }));
}
