using FleetLedger.Core.Requests;

namespace FleetLedger.Core.Tests.Fakes;

public sealed class SampleRequest : Request
{
    public string Name { get; set; } = string.Empty;
}
