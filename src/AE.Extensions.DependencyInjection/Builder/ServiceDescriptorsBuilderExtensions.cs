namespace AE.Extensions.DependencyInjection.Builder
{
    using System.Reflection;

    public static class ServiceDescriptorsBuilderExtensions
    {
        public static ServiceDescriptorsBuilder AddSourceAssemblies(this ServiceDescriptorsBuilder builder, params Assembly[] sourceAssemblies)
        {
            foreach (var sourceAssembly in sourceAssemblies)
            {
                builder.AddSourceAssembly(sourceAssembly);
            }

            return builder;
        }

        public static ServiceDescriptorsBuilder AddSourceAssembly(this ServiceDescriptorsBuilder builder, Assembly sourceAssembly)
        {
            builder.AddTypesProvider(new AssemblyTypeProvider(sourceAssembly));
            return builder;
        }
    }
}