namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    using Abstractions;

    [RepleaceDependency(typeof(RepleacedTestService))]
    public class RepleacingTestService : RepleacedTestService
    {
    }
}