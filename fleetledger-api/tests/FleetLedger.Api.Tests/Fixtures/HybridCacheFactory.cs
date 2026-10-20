using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace FleetLedger.Api.Tests.Fixtures;

// Cache em memória novo a cada chamada: nenhum teste enxerga o estado de outro.
public static class HybridCacheFactory
{
    public static HybridCache Create()
    {
        ServiceCollection services = new();
        services.AddHybridCache();
        return services.BuildServiceProvider().GetRequiredService<HybridCache>();
    }
}
