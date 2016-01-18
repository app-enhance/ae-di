namespace AE.Extensions.DependencyInjection.Builder.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RepleaceDependencyTypeConvention : ITypeConvention
    {
        private readonly Func<IEnumerable<IAttributesProvider>> _attributesProvidersFactory;

        private readonly Lazy<IEnumerable<Type>> _repleacedTypes;

        public RepleaceDependencyTypeConvention(Func<IEnumerable<IAttributesProvider>> attributesProvidersFactory)
        {
            _attributesProvidersFactory = attributesProvidersFactory;
            _repleacedTypes = new Lazy<IEnumerable<Type>>(FindRepleacedTypes);
        }

        public bool IsSatisfiedBy(Type type)
        {
            return _repleacedTypes.Value.Contains(type) == false;
        }

        private IEnumerable<Type> FindRepleacedTypes()
        {
            var repleaceDependencyAttibutes = _attributesProvidersFactory().SelectMany(x => x.GetAttributes<RepleaceDependencyAttribute>());
            return repleaceDependencyAttibutes.Select(x => x.RepleacedType).ToList();
        }
    }
}