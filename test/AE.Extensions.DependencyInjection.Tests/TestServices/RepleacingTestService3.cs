namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    using Abstractions;

    [RepleaceDependency(typeof(RepleacedTestService3))]
    public class RepleacingTestService3 : RepleacedTestService
    {
    }
}