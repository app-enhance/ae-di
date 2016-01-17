namespace AE.Extensions.DependencyInjection.BuildDependencies
{
    using System;
    using System.Collections.Generic;

    public interface ITypesProvider
    {
        IEnumerable<Type> RetrieveTypes();
    }
}