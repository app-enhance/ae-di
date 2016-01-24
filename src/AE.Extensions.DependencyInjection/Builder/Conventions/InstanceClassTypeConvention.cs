namespace AE.Extensions.DependencyInjection.Builder.Conventions
{
    using System;
    using System.Reflection;

    public class InstanceClassTypeSelectionConvention : TypeSelectionConvention
    {
        public override bool DoesSelect(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsClass && !typeInfo.IsAbstract && !typeInfo.IsGenericTypeDefinition;
        }
    }
}