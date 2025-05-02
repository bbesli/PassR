using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;
using PassR.Utilities.OpenApi;

namespace PassR.Utilities.Extensions
{
    /// <summary>
    /// Provides extension methods for registering presentation layer dependencies,
    /// including API versioning, Swagger, authentication, and middleware-related services.
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers common ASP.NET Core services used in the presentation layer,
        /// such as authentication, Swagger generation, problem details, and API versioning.
        /// </summary>
        /// <param name="services">The service collection to register dependencies to.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance for chaining.</returns>
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddAuthentication();
            services.AddAuthorization();

            services.AddEndpointsApiExplorer();

            // Configure API versioning FIRST
            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            // ✅ Configure Swagger AFTER versioning
            services.AddSwaggerGen();
            services.ConfigureOptions<ConfigureSwaggerGenOptions>();

            services.AddHttpContextAccessor();
            services.AddProblemDetails();

            return services;
        }
    }
}
