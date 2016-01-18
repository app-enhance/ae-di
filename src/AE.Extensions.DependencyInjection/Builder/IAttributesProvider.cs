namespace AE.Extensions.DependencyInjection.Builder
{
    using System;
    using System.Collections.Generic;

    public interface IAttributesProvider
    {
        IEnumerable<T> GetAttributes<T>() where T : Attribute;
    }
}