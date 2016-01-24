namespace AE.Extensions.DependencyInjection.Tests.IncorrectAssembly
{
    public interface IInheritMoreThanOnleLifeTimeScopeService : ITransientDependency, IScopedDependency
    {
    }
}