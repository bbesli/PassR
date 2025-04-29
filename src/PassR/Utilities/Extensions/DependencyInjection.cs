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
            services.AddSwaggerGen();

            services.AddHttpContextAccessor();

            // REMARK: If you want to use Controllers, uncomment this.
            // services.AddControllers();

            services.AddProblemDetails();

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            services.ConfigureOptions<ConfigureSwaggerGenOptions>();

            return services;
        }
    }
}
