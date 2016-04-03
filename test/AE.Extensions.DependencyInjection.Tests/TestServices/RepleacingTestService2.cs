namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    using Abstractions;

    [RepleaceDependency(typeof(RepleacedTestService2))]
    public class RepleacingTestService2 : RepleacedTestService
    {
    }
}