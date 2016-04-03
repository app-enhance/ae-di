namespace AE.Extensions.DependencyInjection.Tests.TestServices.MoreThanOneLifetime
{
    using Abstractions;

    public interface IInheritMoreThanOnleLifeTimeScopeService : ITransientDependency, IScopedDependency
    {
    }
}