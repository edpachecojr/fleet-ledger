using FleetLedger.Core.Common.Validation;
using Microsoft.Extensions.Options;

namespace FleetLedger.Api.Options;

public sealed class CrossOriginOptionsValidator : IValidateOptions<CrossOriginOptions>
{
    public ValidateOptionsResult Validate(string? name, CrossOriginOptions options)
    {
        ValidationBuilder rules = new();

        for (int index = 0; index < options.AllowedOrigins.Count; index++)
            ValidateOrigin(rules, index, options.AllowedOrigins[index]);

        return rules.Build().ToOptionsResult();
    }

    private static void ValidateOrigin(ValidationBuilder rules, int index, string origin)
        => rules.When(
            !IsHttpOrigin(origin),
            $"{CrossOriginOptions.SectionName}:AllowedOrigins:{index}",
            $"{CrossOriginOptions.SectionName}:AllowedOrigins:{index} inválido. Recebido: '{origin}'. " +
            "Esperado: origem http(s) absoluta sem caminho, ex.: http://localhost:5173");

    private static bool IsHttpOrigin(string origin)
        => Uri.TryCreate(origin, UriKind.Absolute, out Uri? uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps)
            && uri.AbsolutePath == "/"
            && !origin.EndsWith('/');
}
