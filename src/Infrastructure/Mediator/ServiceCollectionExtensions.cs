using Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Mediator
{
    public static class ServiceCollectionExtensions
    {
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
                var definition = openBehavior.IsGenericType ? openBehavior.GetGenericTypeDefinition() : openBehavior;
                services.Add(new ServiceDescriptor(typeof(IPipelineBehavior<,>), definition, options.HandlerLifetime));
            }

            return services;
        }
    }
}
