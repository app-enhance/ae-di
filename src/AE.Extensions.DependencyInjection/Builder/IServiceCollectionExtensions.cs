namespace AE.Extensions.DependencyInjection.Builder
{
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddFromAssemblies(this IServiceCollection serviceCollection, Assembly[] assebmlies)
        {
            var builder = new ServiceDescriptionsBuilder().AddSourceAssemblies(assebmlies);
            BuildAndFill(serviceCollection, builder);

            return serviceCollection;
        }

        public static IServiceCollection AddFromAssembly(this IServiceCollection serviceCollection, Assembly assembly)
        {
            var builder = new ServiceDescriptionsBuilder().AddSourceAssembly(assembly);
            BuildAndFill(serviceCollection, builder);

            return serviceCollection;
        }

        private static void BuildAndFill(IServiceCollection serviceCollection, ServiceDescriptionsBuilder builder)
        {
            var serviceDescriptions = builder.Build();
            foreach (var descriptor in serviceDescriptions)
            {
                serviceCollection.Add(descriptor);
            }
        }
    }
}