namespace AE.Extensions.DependencyInjection.Tests.TestServices.MoreThanOneLifetime
{
    public interface IInheritMoreThanOnleLifeTimeScopeService : ITransientDependency, IScopedDependency
    {
    }
}