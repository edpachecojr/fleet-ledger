using FleetLedger.Api.Data.Stores.LedgerAccounts;
using FleetLedger.Api.Data.Stores.Vehicles;
using FleetLedger.Api.Handlers.Auth;
using FleetLedger.Api.Handlers.LedgerAccounts;
using FleetLedger.Api.Handlers.Vehicles;
using FleetLedger.Core.Handlers;

namespace FleetLedger.Api.Common.Api;

public static class ApplicationServicesExtension
{
    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddHybridCache();

        builder.Services.AddScoped<IAuthHandler, AuthHandler>();

        builder.Services.AddScoped<ILedgerAccountStore, LedgerAccountStore>();
        builder.Services.AddScoped<ILedgerAccountHandler, LedgerAccountHandler>();

        builder.Services.AddScoped<IVehicleStore, VehicleStore>();
        builder.Services.AddScoped<IVehicleHandler, VehicleHandler>();
    }
}
