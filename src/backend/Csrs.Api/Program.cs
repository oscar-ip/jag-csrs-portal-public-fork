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
Log.Information("Starting up: {Namespace}.Main() ---> .NET Runtime Version: {Version}, OS: {OSDescription}, ProcessArchitecture: {Arch}", 
    namespaceMain,
    RuntimeInformation.FrameworkDescription,
    RuntimeInformation.OSDescription,
    RuntimeInformation.ProcessArchitecture);

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
    // attempt to flush any logs
    Log.Information("Shutting down: {Namespace}.Main()", namespaceMain);
    Serilog.Log.CloseAndFlush();
}
