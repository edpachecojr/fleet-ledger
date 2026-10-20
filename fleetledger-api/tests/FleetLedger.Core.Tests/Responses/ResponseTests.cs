using System.Text.Json;
using FleetLedger.Core.Responses;

namespace FleetLedger.Core.Tests.Responses;

public class ResponseTests
{
    [Theory]
    [InlineData(200, true)]
    [InlineData(201, true)]
    [InlineData(299, true)]
    [InlineData(400, false)]
    [InlineData(404, false)]
    [InlineData(500, false)]
    public void IsSuccess_ForEachStatusCode_MatchesTwoHundredRange(int code, bool expected)
    {
        Response<string> response = new(null, code);

        response.IsSuccess.Should().Be(expected);
    }

    [Fact]
    public void Deserialize_WhenResponseIsAnError_PreservesCodeAndIsSuccess()
    {
        Response<string> original = new(null, 404, "Não encontrado");

        string json = JsonSerializer.Serialize(original);
        Response<string>? roundTrip = JsonSerializer.Deserialize<Response<string>>(json);

        roundTrip!.Code.Should().Be(404);
        roundTrip.IsSuccess.Should().BeFalse();
        roundTrip.Message.Should().Be("Não encontrado");
    }

    [Fact]
    public void Serialize_Always_OmitsIsSuccess()
    {
        string json = JsonSerializer.Serialize(new Response<string>("ok"));

        json.Should().NotContain(nameof(Response<string>.IsSuccess));
    }
}
