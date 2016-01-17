namespace AE.Extensions.DependencyInjection.BuildDependencies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class RepleaceDependencyFilter : ITypeFilter
    {
        private readonly Func<IEnumerable<Type>> _typesFactory;

        private readonly Lazy<IEnumerable<Type>> _repleacedTypes;

        public RepleaceDependencyFilter(Func<IEnumerable<Type>> typesFactory)
        {
            _typesFactory = typesFactory;
            _repleacedTypes = new Lazy<IEnumerable<Type>>(() => FindRepleacedTypes(_typesFactory()));
        }

        public bool IsSatisfiedBy(Type type)
        {
            return _repleacedTypes.Value.Contains(type) == false;
        }

        private IEnumerable<Type> FindRepleacedTypes(IEnumerable<Type> types)
        {
            var repleaceDependencyAttibutes = types.SelectMany(x => x.GetTypeInfo().GetCustomAttributes<RepleaceDependencyAttribute>());
            return repleaceDependencyAttibutes.Select(x => x.RepleacedType).ToList();
        }
    }
}