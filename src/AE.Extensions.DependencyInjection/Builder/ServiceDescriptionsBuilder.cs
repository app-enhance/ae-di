namespace AE.Extensions.DependencyInjection.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Conventions;

    using Microsoft.Extensions.DependencyInjection;

    public class ServiceDescriptionsBuilder
    {
        private readonly List<ITypeSelectionConvention> _typesConventions;

        private readonly List<ITypesProvider> _typesProviders;

        public ServiceDescriptionsBuilder()
        {
            _typesProviders = new List<ITypesProvider>();
            _typesConventions = new List<ITypeSelectionConvention>
                                    {
                                        new InstanceClassTypeSelectionConvention(),
                                        new DependencyTypeSelectionConvention()
                                    };
        }

        public ServiceDescriptionsBuilder(IEnumerable<ITypeSelectionConvention> selectionConventions)
            : this()
        {
            if ((selectionConventions == null) || (selectionConventions.Any() == false))
            {
                throw new ArgumentNullException(nameof(selectionConventions));
            }

            _typesConventions.Clear();
            _typesConventions.AddRange(selectionConventions);
        }

        public ServiceDescriptionsBuilder AddTypesProvider(ITypesProvider typesProvider)
        {
            _typesProviders.Add(typesProvider);
            return this;
        }

        public ServiceDescriptionsBuilder AddTypesConvention(ITypeSelectionConvention selectionConvention)
        {
            _typesConventions.Add(selectionConvention);
            return this;
        }

        public IEnumerable<ServiceDescriptor> Build()
        {
            var typesToRegistration = GetTypesToRegistration();
            return ServicesDescriber.Describe(typesToRegistration);
        }

        private IEnumerable<Type> GetTypesToRegistration()
        {
            var unionConvention = new UnionSelectionConvention(_typesConventions);
            var potentialTypes = SelectPotentialTypes(_typesProviders, unionConvention);
            return GetTypesToRegistration(potentialTypes, unionConvention);
        }

        private IEnumerable<Type> SelectPotentialTypes(IEnumerable<ITypesProvider> typesProviders, ITypeSelector convention)
        {
            return typesProviders.AsParallel().SelectMany(provider => provider.RetrieveTypes(convention));
        }

        private static IEnumerable<Type> GetTypesToRegistration(IEnumerable<Type> potentialTypesToRegister, ITypeSelectionConvention convention)
        {
            return potentialTypesToRegister.AsParallel().Where(convention.DoesPostSelect);
        }

        private class UnionSelectionConvention : ITypeSelectionConvention
        {
            private readonly IEnumerable<ITypeSelectionConvention> _conventionsToUnion;

            internal UnionSelectionConvention(IEnumerable<ITypeSelectionConvention> conventionsToUnion)
            {
                _conventionsToUnion = conventionsToUnion;
            }

            public bool DoesSelect(Type type)
            {
                return _conventionsToUnion.AsParallel().All(x => x.DoesSelect(type));
            }

            public bool DoesPostSelect(Type type)
            {
                return _conventionsToUnion.AsParallel().All(x => x.DoesPostSelect(type));
            }
        }
    }
}