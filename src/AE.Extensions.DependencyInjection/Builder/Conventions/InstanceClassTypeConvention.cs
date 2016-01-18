namespace AE.Extensions.DependencyInjection.Builder.Conventions
{
    using System;
    using System.Reflection;

    public class InstanceClassTypeConvention : ITypeConvention
    {
        public bool IsSatisfiedBy(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericTypeDefinition;
        }
    }
}