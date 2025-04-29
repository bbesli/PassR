using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PassR.Utilities.Endpoints;
using System.Reflection;

namespace PassR.Utilities.Extensions
{
    /// <summary>
    /// Provides extension methods for modular endpoint discovery, registration, and configuration
    /// within ASP.NET Core applications using the <see cref="IEndpoint"/> abstraction.
    /// </summary>
    public static class EndpointExtensions
    {
        /// <summary>
        /// Scans the specified assembly for non-abstract, non-interface implementations of <see cref="IEndpoint"/>
        /// and registers them as transient services for dependency injection.
        /// </summary>
        /// <param name="services">The service collection to register endpoints into.</param>
        /// <param name="assembly">The assembly to scan for endpoint implementations.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
        {
            ServiceDescriptor[] serviceDescriptors = assembly
                .DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                               type.IsAssignableTo(typeof(IEndpoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
                .ToArray();

            services.TryAddEnumerable(serviceDescriptors);

            return services;
        }

        /// <summary>
        /// Maps all registered <see cref="IEndpoint"/> instances to the specified routing builder.
        /// Allows for modular and isolated endpoint configuration, promoting clean architecture practices.
        /// </summary>
        /// <param name="app">The <see cref="WebApplication"/> instance.</param>
        /// <param name="routeGroupBuilder">
        /// Optional <see cref="RouteGroupBuilder"/> to group routes. If null, routes are added directly to the app.
        /// </param>
        /// <returns>The configured <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder MapEndpoints(
            this WebApplication app,
            RouteGroupBuilder? routeGroupBuilder = null)
        {
            IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

            IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

            foreach (IEndpoint endpoint in endpoints)
            {
                endpoint.MapEndpoint(builder);
            }

            return app;
        }

        /// <summary>
        /// Adds permission-based authorization to the given route handler.
        /// This is a shorthand extension for <c>RequireAuthorization(permission)</c>.
        /// </summary>
        /// <param name="app">The route handler builder.</param>
        /// <param name="permission">The name of the required permission policy.</param>
        /// <returns>The updated <see cref="RouteHandlerBuilder"/> instance.</returns>
        public static RouteHandlerBuilder HasPermission(this RouteHandlerBuilder app, string permission)
        {
            return app.RequireAuthorization(permission);
        }
    }
}
