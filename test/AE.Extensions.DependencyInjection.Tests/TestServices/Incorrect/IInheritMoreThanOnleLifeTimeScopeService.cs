namespace AE.Extensions.DependencyInjection.Tests.TestServices.Incorrect
{
    public interface IInheritMoreThanOnleLifeTimeScopeService : ITransientDependency, IScopedDependency
    {
    }
}