namespace AE.Extensions.DependencyInjection.Builder.Conventions
{
    using System;
    using System.Reflection;

    public class DependencyTypeConvention : ITypeConvention
    {
        private readonly Type _dependencyType = typeof(IDependency);

        private readonly Type _notRegisterDependencyType = typeof(INotRegisterDependency);

        public bool IsSatisfiedBy(Type type)
        {
            return _dependencyType.IsAssignableFrom(type) && !_notRegisterDependencyType.IsAssignableFrom(type);
        }
    }
}