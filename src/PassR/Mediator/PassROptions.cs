using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PassR.Mediator
{
    /// <summary>
    /// Defines configuration options for setting up the PassR mediator.
    /// 
    /// <para>
    /// Allows customization of handler lifetimes, registration of assemblies containing handlers,
    /// and addition of open generic pipeline behaviors.
    /// </para>
    /// </summary>
    public class PassROptions
    {
        /// <summary>
        /// Gets or sets the default service lifetime for registered handlers.
        /// The default is <see cref="ServiceLifetime.Scoped"/>.
        /// </summary>
        public ServiceLifetime HandlerLifetime { get; set; } = ServiceLifetime.Scoped;

        /// <summary>
        /// Gets the list of assemblies that will be scanned for request and notification handlers.
        /// </summary>
        public List<Assembly> AssembliesToScan { get; } = new();

        /// <summary>
        /// Gets the list of open generic pipeline behaviors to be registered in the container.
        /// </summary>
        public List<Type> OpenBehaviors { get; } = new();

        /// <summary>
        /// Registers all request and notification handlers found in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan for handlers.</param>
        /// <returns>The updated <see cref="PassROptions"/> instance for fluent chaining.</returns>
        public PassROptions RegisterServicesFromAssembly(Assembly assembly)
        {
            AssembliesToScan.Add(assembly);
            return this;
        }

        /// <summary>
        /// Adds an open generic pipeline behavior (e.g., <c>LoggingBehavior&lt;,&gt;</c>) to the mediator configuration.
        /// </summary>
        /// <param name="behaviorType">The open generic behavior type to register.</param>
        /// <returns>The updated <see cref="PassROptions"/> instance for fluent chaining.</returns>
        public PassROptions AddOpenBehavior(Type behaviorType)
        {
            OpenBehaviors.Add(behaviorType);
            return this;
        }
    }
}
