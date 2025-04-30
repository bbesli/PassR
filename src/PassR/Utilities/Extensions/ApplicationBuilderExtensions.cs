using Microsoft.AspNetCore.Builder;
using PassR.Utilities.Middleware;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;


namespace PassR.Utilities.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring application middleware in an ASP.NET Core application.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Enables Swagger and Swagger UI with support for API versioning.
        /// 
        /// <para>
        /// This method registers Swagger middleware and dynamically adds UI endpoints
        /// for all discovered API version descriptions. Requires <see cref="IApiVersionDescriptionProvider"/>
        /// to be registered in the service container.
        /// </para>
        /// </summary>
        /// <param name="app">The application builder used to configure middleware.</param>
        /// <returns>The updated <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseSwaggerWithUi(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                IReadOnlyList<ApiVersionDescription> descriptions = app.DescribeApiVersions();

                foreach (ApiVersionDescription description in descriptions)
                {
                    string url = $"/swagger/{description.GroupName}/swagger.json";
                    string name = description.GroupName.ToUpperInvariant();

                    options.SwaggerEndpoint(url, name);
                }
            });

            return app;
        }

        /// <summary>
        /// Registers a global exception handling middleware to catch and handle unhandled exceptions gracefully.
        /// 
        /// <para>
        /// This middleware captures exceptions, logs them, and returns standardized error responses
        /// (e.g., for validation, application, or unexpected exceptions).
        /// </para>
        /// </summary>
        /// <param name="app">The application builder used to configure middleware.</param>
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
