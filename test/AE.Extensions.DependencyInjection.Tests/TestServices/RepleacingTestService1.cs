namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    using Abstractions;

    [RepleaceDependency(typeof(RepleacedTestService1))]
    public class RepleacingTestService1 : RepleacedTestService
    {
    }
}