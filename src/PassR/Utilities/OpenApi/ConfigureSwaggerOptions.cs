using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PassR.Utilities.OpenApi
{
    /// <summary>
    /// Configures Swagger documentation options dynamically for each available API version.
    /// 
    /// <para>
    /// This class integrates with the ASP.NET Core API versioning system to ensure that
    /// a separate Swagger document is generated for each discovered <see cref="ApiVersionDescription"/>.
    /// </para>
    /// </summary>
    public class ConfigureSwaggerGenOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerGenOptions"/> class.
        /// </summary>
        /// <param name="provider">The API version description provider used to retrieve available API versions.</param>
        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Configures Swagger documentation options for all API versions.
        /// </summary>
        /// <param name="options">The Swagger options to be configured.</param>
        public void Configure(SwaggerGenOptions options)
        {
            foreach (ApiVersionDescription description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }
        }

        /// <summary>
        /// Configures named Swagger documentation options (calls the non-named version internally).
        /// </summary>
        /// <param name="name">The name of the options instance.</param>
        /// <param name="options">The Swagger options to be configured.</param>
        public void Configure(string? name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        /// <summary>
        /// Creates a versioned <see cref="OpenApiInfo"/> for use in Swagger UI per API version.
        /// </summary>
        /// <param name="apiVersionDescription">The version metadata for the API.</param>
        /// <returns>A configured <see cref="OpenApiInfo"/> instance.</returns>
        private static OpenApiInfo CreateVersionInfo(ApiVersionDescription apiVersionDescription)
        {
            var openApiInfo = new OpenApiInfo
            {
                Title = $"API v{apiVersionDescription.ApiVersion}",
                Version = apiVersionDescription.ApiVersion.ToString()
            };

            if (apiVersionDescription.IsDeprecated)
            {
                openApiInfo.Description += " This API version has been deprecated.";
            }

            return openApiInfo;
        }
    }
}
