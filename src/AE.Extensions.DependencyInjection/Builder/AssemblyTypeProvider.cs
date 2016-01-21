namespace AE.Extensions.DependencyInjection.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Conventions;

    public class AssemblyTypeProvider : ITypesProvider
    {
        private readonly IEnumerable<Type> _sourceTypes;

        public AssemblyTypeProvider(Assembly sourceAssembly)
        {
            _sourceTypes = sourceAssembly.ExportedTypes;
        }

        public IEnumerable<Type> RetrieveTypes(ITypeSelector selector)
        {
            var typesToRegistration = _sourceTypes.Where(selector.DoesSelect).ToList();
            return typesToRegistration;
        }
    }
}