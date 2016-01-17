namespace AE.Extensions.DependencyInjection.BuildDependencies
{
    using System;
    using System.Reflection;

    public class InstanceDependenciesOnlyTypeFilter : ITypeFilter
    {
        private readonly Type _dependencyType = typeof(IDependency);

        private readonly Type _notRegisterDependencyType = typeof(INotRegisterDependency);

        public bool IsSatisfiedBy(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return _dependencyType.IsAssignableFrom(type) && !_notRegisterDependencyType.IsAssignableFrom(type) && typeInfo.IsClass
                   && !typeInfo.IsAbstract && !typeInfo.IsGenericTypeDefinition;
        }
    }
}