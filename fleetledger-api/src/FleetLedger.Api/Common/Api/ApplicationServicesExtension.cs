using FleetLedger.Api.Common.Time;
using FleetLedger.Api.Data.Stores.Customers;
using FleetLedger.Api.Data.Stores.Drivers;
using FleetLedger.Api.Data.Stores.LedgerAccounts;
using FleetLedger.Api.Data.Stores.Vehicles;
using FleetLedger.Api.Handlers.Auth;
using FleetLedger.Api.Handlers.Customers;
using FleetLedger.Api.Handlers.Drivers;
using FleetLedger.Api.Handlers.LedgerAccounts;
using FleetLedger.Api.Handlers.Vehicles;
using FleetLedger.Core.Handlers;

namespace FleetLedger.Api.Common.Api;

public static class ApplicationServicesExtension
{
    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton(TimeProvider.System);
        builder.Services.AddSingleton<IBusinessCalendar, BusinessCalendar>();
        builder.Services.AddHybridCache();

        builder.Services.AddScoped<IAuthHandler, AuthHandler>();

        builder.Services.AddScoped<ILedgerAccountStore, LedgerAccountStore>();
        builder.Services.AddScoped<ILedgerAccountHandler, LedgerAccountHandler>();

        builder.Services.AddScoped<IVehicleStore, VehicleStore>();
        builder.Services.AddScoped<IVehicleHandler, VehicleHandler>();

        builder.Services.AddScoped<ICustomerStore, CustomerStore>();
        builder.Services.AddScoped<ICustomerHandler, CustomerHandler>();

        builder.Services.AddScoped<IDriverStore, DriverStore>();
        builder.Services.AddScoped<IDriverHandler, DriverHandler>();
    }
}
