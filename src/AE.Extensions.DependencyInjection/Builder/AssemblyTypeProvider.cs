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
            if (sourceAssembly == null)
            {
                throw new ArgumentNullException(nameof(sourceAssembly));
            }

            _sourceTypes = sourceAssembly.ExportedTypes;
        }

        public IEnumerable<Type> RetrieveTypes(ITypeSelector selector)
        {
            var choosenSelector = selector ?? new AllSelector();
            var typesToRegistration = _sourceTypes.Where(choosenSelector.DoesSelect).ToList();
            return typesToRegistration;
        }

        private class AllSelector : ITypeSelector
        {
            public bool DoesSelect(Type type)
            {
                return true;
            }
        }
    }
}