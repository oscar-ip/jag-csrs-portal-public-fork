using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddCsrsEnvironmentVariables();

builder.AddJwtBearerAuthentication();
builder.UseSerilog();
builder.AddHealthChecks();
builder.AddInstrumentation();
builder.AddRepositories();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddAutoMapper(typeof(Program).Assembly);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "CSRS Api",

    });

    c.EnableAnnotations();

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

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
