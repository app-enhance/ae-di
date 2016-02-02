namespace AE.Extensions.DependencyInjection.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Conventions;

    using Microsoft.Extensions.DependencyInjection;
    

    public class ExistingServiceCollectionTypesProvider : ITypesProvider
    {
        private readonly IServiceCollection _existingServiceCollection;

        public ExistingServiceCollectionTypesProvider(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            _existingServiceCollection = serviceCollection;
        }

        public IEnumerable<Type> RetrieveTypes(ITypeSelector selector)
        {
            return _existingServiceCollection.Where(x => x.ImplementationType != null).Select(x => x.ImplementationType);
        }
    }
}
