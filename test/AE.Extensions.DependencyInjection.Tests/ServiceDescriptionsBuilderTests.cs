namespace AE.Extensions.DependencyInjection.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Builder;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    public class ServiceDescriptionsBuilderTests
    {
        [Fact]
        public void When_Assembly_has_services_describer_should_describe_them_correct()
        {
            // Arrange
            var assembly = GetTestAssembly();
            var builder = new ServiceDescriptionsBuilder().AddSourceAssembly(assembly);

            // Act
            var serviceDescriptions = builder.Build();

            // Assert
            var description = serviceDescriptions.First();
            Assert.Equal(ServiceLifetime.Scoped, description.Lifetime);
            Assert.Equal(typeof(ITestDependency), description.ServiceType);
            Assert.Equal(typeof(TestServiceDependency), description.ImplementationType);
        }

        [Fact]
        public void When_exists_service_with_RepleaceDependecy_Describer_should_describe_only_this_one()
        {
            // Arrange
            var assembly = GetTestAssembly();
            var builder = new ServiceDescriptionsBuilder().AddSourceAssembly(assembly);

            // Act
            var serviceDescriptions = builder.Build();

            // Assert
            Assert.Contains(
                CreateServiceDescriptor<IRepleaceDependency, RepleacingTestService>(ServiceLifetime.Scoped),
                serviceDescriptions,
                new ServiceDescriptorComparer());
            Assert.DoesNotContain(
                CreateServiceDescriptor<IRepleaceDependency, RepleacedTestService>(ServiceLifetime.Scoped),
                serviceDescriptions,
                new ServiceDescriptorComparer());
        }

        [Fact]
        public void Describer_describe_generics_in_correct_way()
        {
            // Arrange
            var assembly = GetTestAssembly();
            var builder = new ServiceDescriptionsBuilder().AddSourceAssembly(assembly);

            // Act
            var serviceDescriptions = builder.Build();

            // Assert
            Assert.Contains(
                CreateServiceDescriptor<IGeneric<int>, TestGenericService>(ServiceLifetime.Scoped),
                serviceDescriptions,
                new ServiceDescriptorComparer());
        }

        private Assembly GetTestAssembly()
        {
            return typeof(ServiceDescriptionsBuilderTests).GetTypeInfo().Assembly;
        }

        private ServiceDescriptor CreateServiceDescriptor<TInterface, TImplementation>(ServiceLifetime lifetime)
        {
            return new ServiceDescriptor(typeof(TInterface), typeof(TImplementation), lifetime);
        }

        public interface ITestDependency : IScopedDependency
        {
        }

        public class TestServiceDependency : ITestDependency
        {
        }

        public interface IRepleaceDependency : IScopedDependency
        {
        }

        public class RepleacedTestService : IRepleaceDependency
        {
        }

        [RepleaceDependency(typeof(RepleacedTestService))]
        public class RepleacingTestService : RepleacedTestService
        {
        }

        public interface IGeneric<T> : IScopedDependency
        {
        }

        public class TestGenericService : IGeneric<int>
        {
        }

        private class ServiceDescriptorComparer : IEqualityComparer<ServiceDescriptor>
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
}