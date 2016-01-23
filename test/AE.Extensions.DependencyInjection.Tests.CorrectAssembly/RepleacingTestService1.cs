namespace AE.Extensions.DependencyInjection.Tests.CorrectAssembly
{
    [RepleaceDependency(typeof(RepleacedTestService1))]
    public class RepleacingTestService1 : RepleacedTestService
    {
    }
}