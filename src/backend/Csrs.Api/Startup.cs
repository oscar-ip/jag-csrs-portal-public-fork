using MediatR;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Csrs.Api;

public static class Startup
{
    /// <summary>
    /// Configures the application.
    /// </summary>
    /// <param name="builder"></param>
    public static void ConfigureApplication(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddCsrsEnvironmentVariables();

        builder.AddJwtBearerAuthentication();
        builder.UseSerilog();
        builder.AddHealthChecks();
        builder.AddInstrumentation();
        builder.AddServices();  // todo: can these services be configured in ConfigureServices?

        builder.Services.ConfigureServices();
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers().AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        services.AddMediatR(typeof(Program));
        services.AddAutoMapper(typeof(Program).Assembly);

        services.AddRouting(options => options.LowercaseUrls = true);

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Version = "v1",
                Title = "CSRS API",
                Description = "BC Child Support Recalculation Service API",
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement { {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } } });

            c.EnableAnnotations();

            // Set the comments path for the Swagger JSON and UI.
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
    }
}

