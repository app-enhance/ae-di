namespace AE.Extensions.DependencyInjection.Tests
{
    using System.Collections.Generic;

    using Microsoft.Extensions.DependencyInjection;

    public class ServiceDescriptorComparer : IEqualityComparer<ServiceDescriptor>
    {
        public bool Equals(ServiceDescriptor x, ServiceDescriptor y)
        {
            return ReferenceEquals(x, y)
                   || (x.ServiceType == y.ServiceType && x.ImplementationType == y.ImplementationType && x.Lifetime == y.Lifetime);
        }

        public int GetHashCode(ServiceDescriptor obj)
        {
            var hashCode = 17;
            hashCode = (hashCode * 7) + obj.ServiceType.GetHashCode();
            hashCode = (hashCode * 7) + obj.ImplementationType.GetHashCode();
            hashCode = (hashCode * 7) + obj.Lifetime.GetHashCode();

            return hashCode;
        }
    }
}