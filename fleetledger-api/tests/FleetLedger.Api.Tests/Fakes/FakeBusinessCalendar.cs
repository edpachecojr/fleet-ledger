using FleetLedger.Api.Common.Time;

namespace FleetLedger.Api.Tests.Fakes;

public sealed class FakeBusinessCalendar(DateOnly today) : IBusinessCalendar
{
    public static readonly DateOnly ReferenceToday = new(2026, 10, 20);

    public FakeBusinessCalendar() : this(ReferenceToday)
    {
    }

    public DateOnly Today() => today;
}
