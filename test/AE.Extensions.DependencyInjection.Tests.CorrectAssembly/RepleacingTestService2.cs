namespace AE.Extensions.DependencyInjection.Tests.CorrectAssembly
{
    [RepleaceDependency(typeof(RepleacedTestService2))]
    public class RepleacingTestService2 : RepleacedTestService
    {
    }
}