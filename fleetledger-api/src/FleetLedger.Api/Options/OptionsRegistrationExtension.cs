using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Options;

public static class OptionsRegistrationExtension
{
    public static void AddApplicationOptions(this WebApplicationBuilder builder)
    {
        builder.AddValidatedOptions<DatabaseOptions, DatabaseOptionsValidator>(DatabaseOptions.SectionName);
        builder.AddValidatedOptions<JwtOptions, JwtOptionsValidator>(JwtOptions.SectionName);
        builder.AddValidatedOptions<CrossOriginOptions, CrossOriginOptionsValidator>(CrossOriginOptions.SectionName);
        builder.AddValidatedOptions<AdministratorSeedOptions, AdministratorSeedOptionsValidator>(
            AdministratorSeedOptions.SectionName);
        builder.AddValidatedOptions<CalendarOptions, CalendarOptionsValidator>(CalendarOptions.SectionName);
    }

    // ValidateOnStart: configuração inválida derruba o boot, não a primeira requisição.
    private static void AddValidatedOptions<TOptions, TValidator>(this WebApplicationBuilder builder, string sectionName)
        where TOptions : class
        where TValidator : class, IValidateOptions<TOptions>
    {
        builder.Services
            .AddOptions<TOptions>()
            .Bind(builder.Configuration.GetSection(sectionName))
            .ValidateOnStart();

        builder.Services.AddSingleton<IValidateOptions<TOptions>, TValidator>();
    }
}
