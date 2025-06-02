using Microsoft.Extensions.DependencyInjection;
using PassR.Abstractions;
using System.Reflection;

namespace PassR.Mediator
{
    /// <summary>
    /// Provides extension methods for registering PassR mediator services into an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers PassR mediator components, handlers, and pipeline behaviors into the dependency injection container.
        /// 
        /// <para>
        /// Automatically scans assemblies for <see cref="IRequestHandler{TRequest, TResponse}"/> and <see cref="INotificationHandler{TNotification}"/>
        /// implementations and registers them with the configured service lifetime.
        /// </para>
        /// 
        /// <para>
        /// Also allows registering custom open generic pipeline behaviors (e.g., logging, validation) via <see cref="PassROptions"/>.
        /// </para>
        /// </summary>
        /// <param name="services">The service collection to add mediator services to.</param>
        /// <param name="configure">
        /// An optional configuration action to customize registration options
        /// such as handler lifetime, assemblies to scan, and open behaviors.
        /// </param>
        /// <returns>The updated <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddPassR(this IServiceCollection services, Action<PassROptions>? configure = null)
        {
            var options = new PassROptions();
            configure?.Invoke(options);

            services.AddScoped<IPassR, PassR>();

            var assemblies = options.AssembliesToScan.Any()
                ? options.AssembliesToScan
                : new[] { Assembly.GetCallingAssembly() }.ToList();

            foreach (var type in assemblies.SelectMany(a => a.GetTypes()))
            {
                if (type.IsAbstract || type.IsInterface)
                {
                    continue;
                }

                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsGenericType &&
                        (iface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                         iface.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                    {
                        services.Add(new ServiceDescriptor(iface, type, options.HandlerLifetime));
                    }
                }
            }

            foreach (var openBehavior in options.OpenBehaviors)
            {
                var definition = openBehavior.IsGenericType
                    ? openBehavior.GetGenericTypeDefinition()
                    : openBehavior;

                var behaviorInterfaces = definition.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>));

                if (behaviorInterfaces.Any())
                {
                    services.Add(new ServiceDescriptor(typeof(IPipelineBehavior<,>), definition, options.HandlerLifetime));
                }
            }

            return services;
        }
    }
}
