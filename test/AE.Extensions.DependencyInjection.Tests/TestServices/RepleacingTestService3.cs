namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    [RepleaceDependency(typeof(RepleacedTestService3))]
    public class RepleacingTestService3 : RepleacedTestService
    {
    }
}