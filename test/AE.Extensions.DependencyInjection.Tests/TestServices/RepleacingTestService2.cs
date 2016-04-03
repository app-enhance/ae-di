namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    using Abstractions;

    [RepleaceService(typeof(RepleacedTestService2))]
    public class RepleacingTestService2 : RepleacedTestService
    {
    }
}