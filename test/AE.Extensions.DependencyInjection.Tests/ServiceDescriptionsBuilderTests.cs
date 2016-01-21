﻿namespace AE.Extensions.DependencyInjection.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics;
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
            Assert.Contains(
                CreateServiceDescriptor<ITestDependency, TestServiceDependency>(ServiceLifetime.Transient),
                serviceDescriptions,
                new ServiceDescriptorComparer());
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

        [Fact]
        public void Builder_build_descriptors_from_assembly_in_less_that_10ms()
        {
            // Arrange
            var assembly = GetTestAssembly();
            var builder = new ServiceDescriptionsBuilder().AddSourceAssembly(assembly);
            var attempts = 10000;
            var watch = new Stopwatch();
            var elapsedMilliseconds = 0L;

            // Act
            for (int i = 0; i < attempts; i++)
            {
                builder = new ServiceDescriptionsBuilder().AddSourceAssembly(assembly);
                watch.Restart();
                builder.Build();
                watch.Stop();
                elapsedMilliseconds += watch.ElapsedMilliseconds;
            }

            // Assert
            Assert.True(10 >= elapsedMilliseconds / attempts);
        }

        private Assembly GetTestAssembly()
        {
            return typeof(ServiceDescriptionsBuilderTests).GetTypeInfo().Assembly;
        }

        private ServiceDescriptor CreateServiceDescriptor<TInterface, TImplementation>(ServiceLifetime lifetime)
        {
            return new ServiceDescriptor(typeof(TInterface), typeof(TImplementation), lifetime);
        }

        public interface ITestDependency : ITransientDependency
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