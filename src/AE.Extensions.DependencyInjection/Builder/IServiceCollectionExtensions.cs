namespace AE.Extensions.DependencyInjection.Builder
{
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
            var serviceDescriptors = builder.Build();
            foreach (var descriptor in serviceDescriptors)
            {
                serviceCollection.Add(descriptor);
            }
        }
    }
}