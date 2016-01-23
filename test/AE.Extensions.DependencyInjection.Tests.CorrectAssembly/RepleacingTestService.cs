namespace AE.Extensions.DependencyInjection.Tests.CorrectAssembly
{
    [RepleaceDependency(typeof(RepleacedTestService))]
    public class RepleacingTestService : RepleacedTestService
    {
    }
}