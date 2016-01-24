namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    [RepleaceDependency(typeof(RepleacedTestService4))]
    public class RepleacingTestService4 : RepleacedTestService
    {
    }
}