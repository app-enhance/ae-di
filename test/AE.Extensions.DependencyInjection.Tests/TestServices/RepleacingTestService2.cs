﻿namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    [RepleaceDependency(typeof(RepleacedTestService2))]
    public class RepleacingTestService2 : RepleacedTestService
    {
    }
}