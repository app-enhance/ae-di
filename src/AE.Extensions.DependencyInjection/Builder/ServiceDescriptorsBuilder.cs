namespace AE.Extensions.DependencyInjection.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Conventions;

    using Microsoft.Extensions.DependencyInjection;

    public class ServiceDescriptorsBuilder
    {
        private readonly List<ITypeSelectionConvention> _typesConventions;

        private readonly List<ITypesProvider> _typesProviders;

        public ServiceDescriptorsBuilder()
        {
            _typesProviders = new List<ITypesProvider>();
            _typesConventions = new List<ITypeSelectionConvention>
                                    {
                                        new InstanceClassTypeSelectionConvention(),
                                        new DependencyTypeSelectionConvention()
                                    };
        }

        public ServiceDescriptorsBuilder(IEnumerable<ITypeSelectionConvention> selectionConventions)
            : this()
        {
            if ((selectionConventions == null) || (selectionConventions.Any() == false))
            {
                throw new ArgumentNullException(nameof(selectionConventions));
            }

            _typesConventions.Clear();
            _typesConventions.AddRange(selectionConventions);
        }

        public ServiceDescriptorsBuilder AddTypesProvider(ITypesProvider typesProvider)
        {
            if (typesProvider == null)
            {
                throw new ArgumentNullException(nameof(typesProvider));
            }

            _typesProviders.Add(typesProvider);
            return this;
        }

        public ServiceDescriptorsBuilder AddTypesConvention(ITypeSelectionConvention selectionConvention)
        {
            if (selectionConvention == null)
            {
                throw new ArgumentNullException(nameof(selectionConvention));
            }

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

        private static IEnumerable<Type> SelectPotentialTypes(IEnumerable<ITypesProvider> typesProviders, ITypeSelector convention)
        {
            try
            {
                var potentialTypes = typesProviders.AsParallel().SelectMany(provider => provider.RetrieveTypes(convention));
                return potentialTypes.ToList();
            }
            catch (AggregateException e)
            {
                throw e.FlattenTryBubbleUp();
            }
        }

        private static IEnumerable<Type> GetTypesToRegistration(IEnumerable<Type> potentialTypesToRegister, ITypeSelectionConvention convention)
        {
            try
            {
                var typesToregistration = potentialTypesToRegister.AsParallel().Where(convention.DoesPostSelect);
                return typesToregistration.ToList();
            }
            catch (AggregateException e)
            {
                throw e.FlattenTryBubbleUp();
            }
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