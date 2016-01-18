namespace AE.Extensions.DependencyInjection.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Conventions;

    public class AssemblyTypeProvider : ITypesProvider, IAttributesProvider
    {
        private readonly IEnumerable<Type> _sourceTypes;

        public AssemblyTypeProvider(Assembly sourceAssembly)
        {
            _sourceTypes = sourceAssembly.ExportedTypes;
        }

        public IEnumerable<Type> RetrieveTypes(ITypeConvention convention)
        {
            var typesToRegistration = _sourceTypes.Where(convention.IsSatisfiedBy).ToList();
            return typesToRegistration;
        }

        public IEnumerable<T> GetAttributes<T>() where T : Attribute
        {
            return _sourceTypes.SelectMany(x => x.GetTypeInfo().GetCustomAttributes<T>());
        }
    }
}