namespace AE.Extensions.DependencyInjection.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Conventions;

    using Microsoft.Extensions.DependencyInjection;

    public class ServiceDescriptionsBuilder
    {
        private readonly List<ITypeConvention> _typesConventions;

        private readonly List<ITypesProvider> _typesProviders;

        public ServiceDescriptionsBuilder()
        {
            _typesProviders = new List<ITypesProvider>();
            _typesConventions = new List<ITypeConvention>
                                 {
                                     new InstanceClassTypeConvention(),
                                     new DependencyTypeConvention(),
                                     new RepleaceDependencyTypeConvention(() => _typesProviders.OfType<IAttributesProvider>())
                                 };
        }

        public ServiceDescriptionsBuilder(IEnumerable<ITypeConvention> typesFilters)
            : this()
        {
            if ((typesFilters == null) || (typesFilters.Any() == false))
            {
                throw new ArgumentNullException(nameof(typesFilters));
            }

            _typesConventions.Clear();
            _typesConventions.AddRange(typesFilters);
        }

        public ServiceDescriptionsBuilder AddTypesProvider(ITypesProvider typesProvider)
        {
            _typesProviders.Add(typesProvider);
            return this;
        }

        public ServiceDescriptionsBuilder AddTypesFilter(ITypeConvention convention)
        {
            _typesConventions.Add(convention);
            return this;
        }

        public IEnumerable<ServiceDescriptor> Build()
        {
            var typesToRegistration = GetTypesToRegistration();
            return ServicesDescriber.Describe(typesToRegistration);
        }

        private IEnumerable<Type> GetTypesToRegistration()
        {
            var conjunctionFilter = new ConjunctionConvention(_typesConventions);
            return _typesProviders.AsParallel().SelectMany(provider => provider.RetrieveTypes(conjunctionFilter));
        }

        private class ConjunctionConvention : ITypeConvention
        {
            private readonly IEnumerable<ITypeConvention> _filtersToConjunction;

            internal ConjunctionConvention(IEnumerable<ITypeConvention> filtersToConjunction)
            {
                _filtersToConjunction = filtersToConjunction;
            }

            public bool IsSatisfiedBy(Type type)
            {
                return _filtersToConjunction.AsParallel().All(x => x.IsSatisfiedBy(type));
            }
        }
    }
}