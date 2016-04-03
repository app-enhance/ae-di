namespace AE.Extensions.DependencyInjection.Tests.TestServices.MoreThanOneRepleaceDependency
{
    using Abstractions;

    [RepleaceService(typeof(ServiceToRepleace))]
    public class FirstServiceWhichRepleace : ServiceToRepleace
    {
    }
}