using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace PassR.Utilities.Extensions;

public static class ApplicationBootstrapExtensions
{
    /// <summary>
    /// Configures the full PassR HTTP request pipeline including versioned endpoints, Swagger, and error handling.
    /// </summary>
    /// <param name="app">The WebApplication instance.</param>
    /// <param name="version">The default API version to use (e.g., 1).</param>
    /// <param name="endpointAssembly">The assembly to scan for endpoint classes.</param>
    public static void UsePassRPresentation(this WebApplication app, int version, Assembly endpointAssembly)
    {
        // Configure versioned endpoint group
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder versionedGroup = app
            .MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(apiVersionSet);

        app.MapEndpoints(versionedGroup);

        // Swagger + versioned UI
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerWithUi();
        }

        app.UseHttpsRedirection();

        // Global exception handling
        app.UseCustomExceptionHandler();
    }
}
