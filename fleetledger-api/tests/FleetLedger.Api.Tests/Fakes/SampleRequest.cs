using FleetLedger.Core.Requests;

namespace FleetLedger.Api.Tests.Fakes;

public sealed class SampleRequest : Request
{
    public string Name { get; set; } = string.Empty;
}
