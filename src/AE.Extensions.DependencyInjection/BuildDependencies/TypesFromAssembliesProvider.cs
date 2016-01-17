namespace AE.Extensions.DependencyInjection.BuildDependencies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class TypesFromAssembliesProvider : ITypesProvider
    {
        private readonly Func<IEnumerable<Assembly>> _assembliesFactory;

        public TypesFromAssembliesProvider(Func<IEnumerable<Assembly>> assembliesFactory)
        {
            _assembliesFactory = assembliesFactory;
        }

        public IEnumerable<Type> RetrieveTypes(ITypeFilter filter)
        {
            var assembliesToScan = _assembliesFactory();
            var typesToRegistration = assembliesToScan.SelectMany(x => x.ExportedTypes).Where(filter.IsSatisfiedBy).ToList();

            return typesToRegistration;
        }
    }
}