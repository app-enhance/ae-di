namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    [RepleaceDependency(typeof(RepleacedTestService))]
    public class RepleacingTestService : RepleacedTestService
    {
    }
}