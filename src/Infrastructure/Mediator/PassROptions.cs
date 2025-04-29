using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mediator
{
    public class PassROptions
    {
        public ServiceLifetime HandlerLifetime { get; set; } = ServiceLifetime.Scoped;
        public List<Assembly> AssembliesToScan { get; } = new();
        public List<Type> OpenBehaviors { get; } = new();

        public PassROptions RegisterServicesFromAssembly(Assembly assembly)
        {
            AssembliesToScan.Add(assembly);
            return this;
        }

        public PassROptions AddOpenBehavior(Type behaviorType)
        {
            OpenBehaviors.Add(behaviorType);
            return this;
        }
    }
}
