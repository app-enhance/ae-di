namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    using Abstractions;

    [RepleaceService(typeof(RepleacedTestService3))]
    public class RepleacingTestService3 : RepleacedTestService
    {
    }
}