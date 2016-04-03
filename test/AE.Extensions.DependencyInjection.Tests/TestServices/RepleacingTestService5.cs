namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    using Abstractions;

    [RepleaceDependency(typeof(RepleacedTestService5))]
    public class RepleacingTestService5 : RepleacedTestService
    {
    }
}