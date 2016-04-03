namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    using Abstractions;

    [RepleaceService(typeof(RepleacedTestService5))]
    public class RepleacingTestService5 : RepleacedTestService
    {
    }
}