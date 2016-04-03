namespace AE.Extensions.DependencyInjection.Tests.TestServices.MoreThanOneRepleaceDependency
{
    using Abstractions;

    [RepleaceDependency(typeof(ServiceToRepleace))]
    public class FirstServiceWhichRepleace : ServiceToRepleace
    {
    }
}