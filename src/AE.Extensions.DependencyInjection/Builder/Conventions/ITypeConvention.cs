namespace AE.Extensions.DependencyInjection.Builder.Conventions
{
    using System;

    public interface ITypeConvention
    {
        bool IsSatisfiedBy(Type type);
    }
}