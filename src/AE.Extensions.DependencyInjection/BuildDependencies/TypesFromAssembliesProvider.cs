namespace AE.Extensions.DependencyInjection.BuildDependencies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public class TypesFromAssembliesProvider : ITypesProvider
    {
        private readonly Func<IEnumerable<Assembly>> _assembliesFactory;

        private readonly Type _dependencyType = typeof(IDependency);

        private readonly Type _notRegisterDependencyType = typeof(INotRegisterDependency);

        public TypesFromAssembliesProvider(Func<IEnumerable<Assembly>> assembliesFactory)
        {
            _assembliesFactory = assembliesFactory;
        }

        public IEnumerable<Type> RetrieveTypes()
        {
            var assembliesToScan = _assembliesFactory();
            var typesToRegistration = assembliesToScan.SelectMany(x => x.ExportedTypes).Where(IsTypeToAutoRegistration).ToList();

            return typesToRegistration;
        }

        private bool IsTypeToAutoRegistration(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return _dependencyType.IsAssignableFrom(type) && !_notRegisterDependencyType.IsAssignableFrom(type) && typeInfo.IsClass
                   && !typeInfo.IsAbstract && !typeInfo.IsGenericTypeDefinition;
        }
    }
}