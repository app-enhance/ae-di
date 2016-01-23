namespace AE.Extensions.DependencyInjection.Tests.CorrectAssembly
{
    [RepleaceDependency(typeof(RepleacedTestService4))]
    public class RepleacingTestService4 : RepleacedTestService
    {
    }
}