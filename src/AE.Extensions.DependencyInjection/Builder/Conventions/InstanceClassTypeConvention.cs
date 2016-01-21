namespace AE.Extensions.DependencyInjection.Builder.Conventions
{
    using System;
    using System.Reflection;

    public class InstanceClassTypeSelectionConvention : ITypeSelectionConvention
    {
        public bool DoesSelect(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericTypeDefinition;
        }

        public bool DoesPostSelect(Type type)
        {
            return true;
        }
    }
}