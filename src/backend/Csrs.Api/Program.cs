using Csrs.Api;
using System.Reflection;
using System.Runtime.InteropServices;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
var namespaceMain = typeof(Csrs.Api.Startup).Namespace ?? "UnknownNamespace";
Log.Information("Starting up: {Namespace} ---> .NET Runtime Version: {Version}, OS: {OSDescription}, ProcessArchitecture: {Arch}", 
    namespaceMain,
    RuntimeInformation.FrameworkDescription,
    RuntimeInformation.OSDescription,
    RuntimeInformation.ProcessArchitecture);

// Log important environment variables
var envVars = Environment.GetEnvironmentVariables();
var sortedEnvVars = envVars.Cast<System.Collections.DictionaryEntry>()
    .Where(e => !string.IsNullOrEmpty(e.Key?.ToString()))
    .OrderBy(e => e.Key.ToString(), StringComparer.OrdinalIgnoreCase);

var envLogBuilder = new System.Text.StringBuilder();
envLogBuilder.AppendLine("\n  All Environment Variables (excluding sensitive info):");

foreach (var env in sortedEnvVars)
{
    var key = env.Key?.ToString();
    var value = env.Value?.ToString();
    var lowerKey = key.ToLowerInvariant();
    if (lowerKey.Contains("password") || lowerKey.Contains("username") || lowerKey.Contains("token") || lowerKey.Contains("secret"))
    {
        envLogBuilder.AppendLine($"\t{key}: [REDACTED]");
        continue;
    }
    envLogBuilder.AppendLine($"\t{key}: {value}");
}
Log.Debug("{EnvDump}", envLogBuilder.ToString());


builder.ConfigureApplication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
// Prometheus Scraping Endpoint is not currently working, comment out for now
//app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseAuthentication();
app.UseAuthorization();
app.AddHealthChecks();

app.MapControllers();

try
{
    app.Run();
}
finally
{
    Log.Information("Shutting down: {Namespace}", namespaceMain);
    // attempt to flush any logs
    Serilog.Log.CloseAndFlush();
}
