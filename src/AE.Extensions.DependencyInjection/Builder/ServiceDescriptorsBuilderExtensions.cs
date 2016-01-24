namespace AE.Extensions.DependencyInjection.Builder
{
    using System.Reflection;

    public static class ServiceDescriptorsBuilderExtensions
    {
        public static ServiceDescriptionsBuilder AddSourceAssemblies(this ServiceDescriptionsBuilder builder, params Assembly[] sourceAssemblies)
        {
            foreach (var sourceAssembly in sourceAssemblies)
            {
                builder.AddSourceAssembly(sourceAssembly);
            }

            return builder;
        }

        public static ServiceDescriptionsBuilder AddSourceAssembly(this ServiceDescriptionsBuilder builder, Assembly sourceAssembly)
        {
            builder.AddTypesProvider(new AssemblyTypeProvider(sourceAssembly));
            return builder;
        }
    }
}