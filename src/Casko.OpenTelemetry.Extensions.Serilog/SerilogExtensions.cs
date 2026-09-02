using Microsoft.Extensions.Configuration;

namespace Casko.OpenTelemetry.Extensions.Serilog;

/// <summary>
/// Configures Serilog OTLP export through application configuration.
/// </summary>
public static class SerilogExtensions
{
    /// <summary>
    /// Adds the Serilog OpenTelemetry sink when an OTLP endpoint is configured.
    /// </summary>
    public static IConfigurationManager AddCaskoOpenTelemetrySink(this IConfigurationManager configuration)
    {
        var otlpEndpoint = configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
        if (string.IsNullOrWhiteSpace(otlpEndpoint))
        {
            return configuration;
        }

        configuration.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Serilog:Using:0"] = "Serilog.Sinks.OpenTelemetry",
            ["Serilog:WriteTo:1:Name"] = "OpenTelemetry",
            ["Serilog:WriteTo:1:Args:Endpoint"] = otlpEndpoint
        });

        return configuration;
    }
}
