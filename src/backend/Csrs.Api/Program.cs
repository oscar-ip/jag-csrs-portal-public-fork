using Csrs.Api;

var builder = WebApplication.CreateBuilder(args);
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
    Serilog.Log.CloseAndFlush();
}
