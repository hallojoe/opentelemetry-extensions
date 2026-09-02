# Casko OpenTelemetry Extensions

Small, opinionated extensions for adding OpenTelemetry instrumentation and Serilog OTLP logging to ASP.NET Core applications.

## Packages

| Package | Adds |
| --- | --- |
| `Casko.OpenTelemetry.Extensions.AspNetCore` | ASP.NET Core, HTTP client, and runtime metrics instrumentation; ASP.NET Core and HTTP client tracing instrumentation. |
| `Casko.OpenTelemetry.Extensions.Serilog` | Serilog's OpenTelemetry sink, configured from the OTLP endpoint. |

Install the package or packages your application needs:

```bash
dotnet add package Casko.OpenTelemetry.Extensions.AspNetCore
dotnet add package Casko.OpenTelemetry.Extensions.Serilog
```

## ASP.NET Core instrumentation

Call `AddCaskoOpenTelemetry` immediately after creating the `WebApplicationBuilder`:

```csharp
using Casko.OpenTelemetry.Extensions.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddCaskoOpenTelemetry();

var app = builder.Build();
app.MapGet("/", () => "Hello, world!");
app.Run();
```

This configures metrics for ASP.NET Core, outgoing HTTP clients, and runtime activity, plus traces for ASP.NET Core and outgoing HTTP clients.

## Send traces and metrics to an OTLP collector

Set `OTEL_EXPORTER_OTLP_ENDPOINT` to enable OTLP export. If it is not set, instrumentation remains configured but no OTLP exporter is added.

```bash
export OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317
```

For example, when running locally:

```bash
OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317 dotnet run
```

## Send Serilog logs to an OTLP collector

Install `Casko.OpenTelemetry.Extensions.Serilog`, set the same endpoint, and add the sink configuration before configuring Serilog from `builder.Configuration`:

```csharp
using Casko.OpenTelemetry.Extensions.Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCaskoOpenTelemetrySink();

// Configure Serilog from builder.Configuration in your normal application setup.
```

When `OTEL_EXPORTER_OTLP_ENDPOINT` has a value, `AddCaskoOpenTelemetrySink` adds the `Serilog.Sinks.OpenTelemetry` sink and sets its endpoint to that value. When the endpoint is missing or empty, it makes no configuration changes.

## Combined setup

Use both packages to export traces, metrics, and Serilog logs to the same collector:

```csharp
using Casko.OpenTelemetry.Extensions.AspNetCore;
using Casko.OpenTelemetry.Extensions.Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddCaskoOpenTelemetrySink();
builder.AddCaskoOpenTelemetry();

// Configure Serilog from builder.Configuration in your normal application setup.

var app = builder.Build();
app.Run();
```

Set the endpoint once:

```bash
OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317 dotnet run
```
