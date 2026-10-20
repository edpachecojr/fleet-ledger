using FleetLedger.Api.Common.Api;
using FleetLedger.Api.Common.Health;
using FleetLedger.Api.Common.Observability;
using FleetLedger.Api.Common.Security;
using FleetLedger.Api.Data;
using FleetLedger.Api.Endpoints;
using FleetLedger.Api.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddObservability();
builder.AddApplicationOptions();
builder.AddValidators();
builder.AddSecurity();
builder.AddDataContexts();
builder.AddHealthMonitoring();
builder.AddCrossOrigin();
builder.AddHttpContract();
builder.AddApplicationServices();

WebApplication app = builder.Build();

app.UseObservability();
app.UseExceptionHandler();
app.UseCors(CorsPolicies.AllowedClients);
app.UseSecurity();
app.MapEndpoints();

if (app.Environment.IsDevelopment())
    app.ConfigureDevEnvironment();

await app.ApplyPendingMigrationsAsync();
await app.RunAsync();

public partial class Program;
