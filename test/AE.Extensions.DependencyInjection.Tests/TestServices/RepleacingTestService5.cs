﻿namespace AE.Extensions.DependencyInjection.Tests.TestServices
{
    [RepleaceDependency(typeof(RepleacedTestService5))]
    public class RepleacingTestService5 : RepleacedTestService
    {
    }
}