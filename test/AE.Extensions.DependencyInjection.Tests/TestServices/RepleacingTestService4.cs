namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    using Abstractions;

    [RepleaceService(typeof(RepleacedTestService4))]
    public class RepleacingTestService4 : RepleacedTestService
    {
    }
}