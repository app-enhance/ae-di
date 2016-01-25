namespace AE.Extensions.DependencyInjection.Tests.TestServices.MoreThanOneRepleaceDependency
{
    [RepleaceDependency(typeof(ServiceToRepleace))]
    public class FirstServiceWhichRepleace : ServiceToRepleace
    {
    }
}