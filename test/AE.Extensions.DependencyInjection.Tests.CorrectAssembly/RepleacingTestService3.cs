namespace AE.Extensions.DependencyInjection.Tests.CorrectAssembly
{
    [RepleaceDependency(typeof(RepleacedTestService3))]
    public class RepleacingTestService3 : RepleacedTestService
    {
    }
}