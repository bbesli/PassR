using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PassR.Utilities.Endpoints;
using System.Reflection;

namespace PassR.Utilities.Extensions;

/// <summary>
/// Provides extension methods to configure the PassR HTTP request pipeline,
/// including dynamic API versioning, Swagger integration, and global error handling.
/// </summary>
public static class ApplicationBootstrapExtensions
{
    /// <summary>
    /// Configures the PassR HTTP request pipeline with dynamic API version discovery,
    /// versioned endpoint registration, Swagger UI integration, and exception handling.
    /// </summary>
    /// <param name="app">The <see cref="WebApplication"/> instance.</param>
    /// <param name="endpointAssembly">
    /// The assembly to scan for endpoint classes that implement <see cref="IEndpoint"/>
    /// and are annotated with <c>[ApiVersion]</c> attributes.
    /// </param>
    /// <remarks>
    /// - Automatically discovers all API versions used in the application.
    /// - Groups endpoints by version under routes like <c>/api/v{version}</c>.
    /// - Configures Swagger UI to display endpoints for each API version.
    /// </remarks>
    public static void UsePassRPresentation(this WebApplication app, Assembly endpointAssembly)
    {
        // 1. Discover all registered IEndpoint instances from DI
        var allEndpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        // 2. Extract distinct ApiVersions
        var versionTypes = allEndpoints
            .SelectMany(e => e.GetType().GetCustomAttributes<PassR.Utilities.Attributes.ApiVersionAttribute>())
            .Select(a => new ApiVersion(a.Version))
            .Distinct()
            .OrderBy(v => v.MajorVersion)
            .ToList();

        // 3. Register all discovered versions into ApiVersionSet
        var apiVersionSetBuilder = app.NewApiVersionSet()
            .ReportApiVersions();

        foreach (var version in versionTypes)
            apiVersionSetBuilder.HasApiVersion(version);

        var builtApiVersionSet = apiVersionSetBuilder.Build();

        // 4. Map endpoints by version
        foreach (var version in versionTypes)
        {
            var versionedGroup = app
                .MapGroup("/api/v{version:apiVersion}")
                .WithApiVersionSet(builtApiVersionSet)
                .HasApiVersion(version);

            app.MapEndpointsByVersion(versionedGroup, version);
        }

        // 5. Swagger + middleware
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerWithUi();
        }

        app.UseHttpsRedirection();
        app.UseCustomExceptionHandler();
    }
}
