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

        private readonly List<ITypeFilter> _typesfilterss;

        private readonly List<ITypesProvider> _typesProviders;

        public ServiceDescriptionsBuilder()
        {
            _sourceAssemblies = new HashSet<Assembly>();
            _typesProviders = new List<ITypesProvider> { new TypesFromAssembliesProvider(() => _sourceAssemblies) };
            _typesfilterss = new List<ITypeFilter>
                                 {
                                     new InstanceDependenciesOnlyTypeFilter(),
                                     new RepleaceDependencyFilter(() => _sourceAssemblies.SelectMany(x => x.ExportedTypes))
                                 };
        }

        public ServiceDescriptionsBuilder(IEnumerable<ITypesProvider> typesProviders)
            : this()
        {
            ConfigureTypeProviders(typesProviders);
        }

        public ServiceDescriptionsBuilder(IEnumerable<ITypeFilter> typesFilters)
            : this()
        {
            ConfigureTypeFilters(typesFilters);
        }

        public ServiceDescriptionsBuilder(IEnumerable<ITypeFilter> typesFilters, IEnumerable<ITypesProvider> typesProviders)
            : this()
        {
            ConfigureTypeProviders(typesProviders);
            ConfigureTypeFilters(typesFilters);
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

        public ServiceDescriptionsBuilder AddTypesFilter(ITypeFilter filter)
        {
            _typesfilterss.Add(filter);
            return this;
        }

        public IEnumerable<ServiceDescriptor> Build()
        {
            var typesToRegistration = GetTypesToRegistration();
            return ServicesDescriber.Describe(typesToRegistration);
        }

        private IEnumerable<Type> GetTypesToRegistration()
        {
            var conjunctionFilter = new ConjunctionFilter(_typesfilterss);
            return _typesProviders.SelectMany(provider => provider.RetrieveTypes(conjunctionFilter));
        }

        private void ConfigureTypeProviders(IEnumerable<ITypesProvider> typesProviders)
        {
            if ((typesProviders == null) || (typesProviders.Any() == false))
            {
                throw new ArgumentNullException(nameof(typesProviders));
            }

            _typesProviders.Clear();
            _typesProviders.AddRange(typesProviders);
        }

        private void ConfigureTypeFilters(IEnumerable<ITypeFilter> typesFilters)
        {
            if ((typesFilters == null) || (typesFilters.Any() == false))
            {
                throw new ArgumentNullException(nameof(typesFilters));
            }

            _typesfilterss.Clear();
            _typesfilterss.AddRange(typesFilters);
        }

        private class ConjunctionFilter : ITypeFilter
        {
            private readonly IEnumerable<ITypeFilter> _filtersToConjunction;

            internal ConjunctionFilter(IEnumerable<ITypeFilter> filtersToConjunction)
            {
                _filtersToConjunction = filtersToConjunction;
            }

            public bool IsSatisfiedBy(Type type)
            {
                return _filtersToConjunction.All(x => x.IsSatisfiedBy(type));
            }
        }
    }
}