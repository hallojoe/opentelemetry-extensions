using Microsoft.AspNetCore.Builder;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Casko.OpenTelemetry.Extensions.AspNetCore;

/// <summary>
/// Configures OpenTelemetry defaults for ASP.NET Core applications.
/// </summary>
public static class OpenTelemetryExtensions
{
    /// <summary>
    /// Adds ASP.NET Core, HTTP client, and runtime instrumentation. When an OTLP endpoint is configured,
    /// traces and metrics are exported to it.
    /// </summary>
    public static WebApplicationBuilder AddOpinionatedOpenTelemetry(this WebApplicationBuilder builder)
    {
        var hasOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        var openTelemetryBuilder = Microsoft.Extensions.DependencyInjection.OpenTelemetryServicesExtensions
            .AddOpenTelemetry(builder.Services)
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation())
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation());

        if (hasOtlpExporter)
        {
            openTelemetryBuilder
                .WithMetrics(metrics => metrics.AddOtlpExporter())
                .WithTracing(tracing => tracing.AddOtlpExporter());
        }

        return builder;
    }
}
