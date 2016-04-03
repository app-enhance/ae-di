namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    using Abstractions;

    [RepleaceService(typeof(RepleacedTestService))]
    public class RepleacingTestService : RepleacedTestService
    {
    }
}