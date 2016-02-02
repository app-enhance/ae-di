namespace AE.Extensions.DependencyInjection.Builder
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFromAssemblies(this IServiceCollection serviceCollection, Assembly[] assebmlies)
        {
            var builder = new ServiceDescriptorsBuilder().AddSourceAssemblies(assebmlies);
            BuildAndFill(serviceCollection, builder);

            return serviceCollection;
        }

        public static IServiceCollection AddFromAssembly(this IServiceCollection serviceCollection, Assembly assembly)
        {
            var builder = new ServiceDescriptorsBuilder().AddSourceAssembly(assembly);
            BuildAndFill(serviceCollection, builder);

            return serviceCollection;
        }

        private static void BuildAndFill(IServiceCollection serviceCollection, ServiceDescriptorsBuilder builder)
        {
            builder.AddTypesProvider(new ExistingServiceCollectionTypesProvider(serviceCollection));
            var serviceDescriptors = builder.Build();

            MergeServiceDescriptions(serviceCollection, serviceDescriptors);
        }

        private static void MergeServiceDescriptions(IServiceCollection serviceCollection, IEnumerable<ServiceDescriptor> serviceDescriptors)
        {
            var excludedServiceDescriptors = serviceCollection.Where(service =>  serviceDescriptors.Contains(service) == false);
            foreach (var descriptor in excludedServiceDescriptors)
            {
                serviceCollection.Remove(descriptor);
            }

            foreach (var descriptor in serviceDescriptors)
            {
                serviceCollection.Add(descriptor);
            }
        }

        private static bool Contains(this IEnumerable<ServiceDescriptor> serviceDescriptors, ServiceDescriptor serviceDescriptor)
        {
            return serviceDescriptors.Any(x => x.ImplementationType == serviceDescriptor.ImplementationType);
        }
    }
}