namespace AE.Extensions.DependencyInjection.Builder.Conventions
{
    using System;

    public interface ITypeSelector
    {
        bool DoesSelect(Type type);
    }
}