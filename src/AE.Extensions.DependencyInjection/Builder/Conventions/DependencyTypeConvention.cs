namespace AE.Extensions.DependencyInjection.Builder.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class DependencyTypeSelectionConvention : ITypeSelectionConvention
    {
        private readonly Type _dependencyType = typeof(IDependency);

        private readonly Type _notRegisterDependencyType = typeof(INotRegisterDependency);

        private readonly HashSet<Type> _repleacedDependencies;

        public DependencyTypeSelectionConvention()
        {
            _repleacedDependencies = new HashSet<Type>();
        }

        public bool DoesSelect(Type type)
        {
            var doesRegister = _dependencyType.IsAssignableFrom(type) && !_notRegisterDependencyType.IsAssignableFrom(type);
            if (doesRegister)
            {
                MemorizeRepleacedDependencyIfExist(type);
            }

            return doesRegister;
        }

        public bool DoesPostSelect(Type type)
        {
            return _repleacedDependencies.Contains(type) == false;
        }

        private void MemorizeRepleacedDependencyIfExist(Type type)
        {
            var attribute = type.GetTypeInfo().GetCustomAttribute<RepleaceDependencyAttribute>();
            if (attribute != null)
            {
                _repleacedDependencies.Add(attribute.RepleacedType);
            }
        }
    }
}