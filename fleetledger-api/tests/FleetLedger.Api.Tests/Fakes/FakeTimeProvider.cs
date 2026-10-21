namespace FleetLedger.Api.Tests.Fakes;

public sealed class FakeTimeProvider(DateTimeOffset utcNow) : TimeProvider
{
    public static readonly DateTimeOffset ReferenceNow = new(2026, 10, 20, 12, 0, 0, TimeSpan.Zero);

    public FakeTimeProvider() : this(ReferenceNow)
    {
    }

    public override DateTimeOffset GetUtcNow() => utcNow;
}
