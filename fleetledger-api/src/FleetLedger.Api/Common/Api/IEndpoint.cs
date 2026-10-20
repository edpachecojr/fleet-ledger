namespace FleetLedger.Api.Common.Api;

// static abstract: o mapeamento não precisa de instância do endpoint.
public interface IEndpoint
{
    static abstract void Map(IEndpointRouteBuilder app);
}
