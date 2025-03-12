using System.Text;
using CleanArchitecture.Application;
using CleanArchitecture.Application.Common;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitecture.Web.Extensions;

public static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder, AppSettings appsettings)
    {
        builder.Services.AddInfrastructuresService(appsettings);
        builder.Services.AddApplicationService(appsettings);
        builder.Services.AddWebAPIService(appsettings);

        return builder.Build();
    }

    public static async Task<WebApplication> ConfigurePipelineAsync(this WebApplication app, AppSettings appsettings)
    {
        using var loggerFactory = LoggerFactory.Create(builder => { });
        using var scope = app.Services.CreateScope();

        // Initialize database if not using in-memory database
        if (!appsettings.UseInMemoryDatabase)
        {
            var initialize = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
            await initialize.InitializeAsync();
        }

        // Exception Handler Middleware First
        // Ensure the required services are added
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        app.ConfigureExceptionHandler(logger);

        //// Global Exception Middleware
        //app.UseMiddleware<GlobalExceptionMiddleware>();


        // Logging and performance tracking middlewares
        app.UseMiddleware<LoggingMiddleware>();
        app.UseMiddleware<PerformanceMiddleware>();

        // Redirect HTTP requests to HTTPS
        app.UseHttpsRedirection();

        // Set up CORS
        app.UseCors("AllowSpecificOrigin");

        // Swagger UI setup for API documentation
        app.UseSwagger(appsettings);

        // Health check configuration
        app.ConfigureHealthCheck();

        // Authentication and Authorization middlewares
        app.UseAuthentication();
        app.UseAuthorization();

        // Map controllers to routes
        app.MapControllers();

        return app;
    }

}
