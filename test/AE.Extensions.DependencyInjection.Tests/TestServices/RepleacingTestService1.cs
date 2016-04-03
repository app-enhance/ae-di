namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    using Abstractions;

    [RepleaceService(typeof(RepleacedTestService1))]
    public class RepleacingTestService1 : RepleacedTestService
    {
    }
}