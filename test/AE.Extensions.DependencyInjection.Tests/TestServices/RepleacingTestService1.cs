namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    [RepleaceDependency(typeof(RepleacedTestService1))]
    public class RepleacingTestService1 : RepleacedTestService
    {
    }
}