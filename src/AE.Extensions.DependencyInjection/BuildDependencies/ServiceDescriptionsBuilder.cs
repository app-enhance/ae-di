namespace AE.Extensions.DependencyInjection.BuildDependencies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    public class ServiceDescriptionsBuilder
    {
        private readonly HashSet<Assembly> _sourceAssemblies;

        private readonly List<ITypesProvider> _typesProviders;

        public ServiceDescriptionsBuilder()
        {
            _sourceAssemblies = new HashSet<Assembly>();
            _typesProviders = new List<ITypesProvider> { new TypesFromAssembliesProvider(() => _sourceAssemblies) };
        }

        public ServiceDescriptionsBuilder(IEnumerable<ITypesProvider> typesProviders)
            : this()
        {
            if ((typesProviders == null) || (typesProviders.Any() == false))
            {
                throw new ArgumentNullException(nameof(typesProviders));
            }

            _typesProviders.Clear();
            _typesProviders.AddRange(typesProviders);
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

        public ServiceDescriptionsBuilder AddTypesProvider(ITypesProvider typesProvider)
        {
            _typesProviders.Add(typesProvider);
            return this;
        }

        public IEnumerable<ServiceDescriptor> Build()
        {
            var typesToRegistration = GetTypesToRegistration();

            var filteredTypes = RemoveRepleacedTypes(typesToRegistration);

            return ServicesDescriber.Describe(filteredTypes);
        }

        private IEnumerable<Type> GetTypesToRegistration()
        {
            return _typesProviders.SelectMany(provider => provider.RetrieveTypes());
        }

        private IEnumerable<Type> RemoveRepleacedTypes(IEnumerable<Type> typesToRegistration)
        {
            var repleacedTypes = FindRepleacedTypes(typesToRegistration);
            return typesToRegistration.Where(x => repleacedTypes.Contains(x) == false);
        }

        private IEnumerable<Type> FindRepleacedTypes(IEnumerable<Type> types)
        {
            var repleaceDependencyAttibutes = types.SelectMany(x => x.GetTypeInfo().GetCustomAttributes<RepleaceDependencyAttribute>());
            return repleaceDependencyAttibutes.Select(x => x.RepleacedType).ToList();
        }
    }
}