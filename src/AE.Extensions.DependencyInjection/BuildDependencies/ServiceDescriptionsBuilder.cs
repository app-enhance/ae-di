namespace AE.Extensions.DependencyInjection.BuildDependencies
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    public class ServiceDescriptionsBuilder
    {
        private readonly HashSet<Assembly> _sourceAssemblies;

        public ServiceDescriptionsBuilder()
        {
            _sourceAssemblies = new HashSet<Assembly>();
        }

        public ServiceDescriptionsBuilder AddSourceAssembly(Assembly sourceAssembly)
        {
            _sourceAssemblies.Add(sourceAssembly);
            return this;
        }

        public ServiceDescriptionsBuilder AddSourceAssemblies(params Assembly[] sourceAssemblies)
        {
            _sourceAssemblies.UnionWith(sourceAssemblies);
            return this;
        }

        public IEnumerable<ServiceDescriptor> Build()
        {
            return ServicesDescriber.DescribeFromAssemblies(_sourceAssemblies.ToArray());
        }
    }
}