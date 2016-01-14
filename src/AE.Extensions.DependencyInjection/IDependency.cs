namespace AE.Extensions.DependencyInjection
{
    /// <summary>
    ///     Base interface for services as dependencies
    /// </summary>
    public interface IDependency
    {
    }

    /// <summary>
    ///     Base interface for services that are instantiated per application.
    /// </summary>
    public interface ISingletonDependency : IDependency
    {
    }

    /// <summary>
    ///     Base interface for services that are instantiated per unit of work (i.e. web request).
    /// </summary>
    public interface IScopedDependency : IDependency
    {
    }

    /// <summary>
    ///     Base interface for services that are instantiated per usage.
    /// </summary>
    public interface ITransientDependency : IDependency
    {
    }

    /// <summary>
    ///     Component which inherit by this interface will not registered in DI container
    /// </summary>
    public interface INotRegisterDependency
    {
    }
}