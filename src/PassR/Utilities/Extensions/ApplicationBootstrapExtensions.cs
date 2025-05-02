using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using PassR.Utilities.Endpoints;

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
        // Discover API versions from endpoint class attributes
        var versionTypes = endpointAssembly
            .DefinedTypes
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsAbstract)
            .SelectMany(t => t.GetCustomAttributes<PassR.Utilities.Attributes.ApiVersionAttribute>())
            .Select(a => new ApiVersion(a.Version))
            .Distinct()
            .OrderBy(v => v.MajorVersion)
            .ToList();

        // Register all discovered versions
        var apiVersionSet = app.NewApiVersionSet()
            .ReportApiVersions();

        foreach (var v in versionTypes)
            apiVersionSet.HasApiVersion(v);

        var builtApiVersionSet = apiVersionSet.Build();

        // Map endpoints grouped by version
        foreach (var version in versionTypes)
        {
            var versionedGroup = app 
                .MapGroup("api/v{version:apiVersion}")
                .WithApiVersionSet(builtApiVersionSet)
                .HasApiVersion(version);

            app.MapEndpointsByVersion(versionedGroup, version);
        }

        // Enable Swagger UI only in development
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerWithUi();
        }
        
        app.UseHttpsRedirection();
        app.UseCustomExceptionHandler();
    }
}
