namespace AE.Extensions.DependencyInjection.Builder
{
    using System;
    using System.Collections.Generic;

    using Conventions;

    public interface ITypesProvider
    {
        IEnumerable<Type> RetrieveTypes(ITypeSelector selector);
    }
}