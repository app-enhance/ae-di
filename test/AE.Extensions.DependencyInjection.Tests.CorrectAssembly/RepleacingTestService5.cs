namespace AE.Extensions.DependencyInjection.Tests.CorrectAssembly
{
    [RepleaceDependency(typeof(RepleacedTestService5))]
    public class RepleacingTestService5 : RepleacedTestService
    {
    }
}