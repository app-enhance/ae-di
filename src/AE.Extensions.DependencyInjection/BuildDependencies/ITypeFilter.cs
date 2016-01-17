namespace AE.Extensions.DependencyInjection.BuildDependencies
{
    using System;

    public interface ITypeFilter
    {
        bool IsSatisfiedBy(Type type);
    }
}