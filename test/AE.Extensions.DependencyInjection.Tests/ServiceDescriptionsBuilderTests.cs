namespace AE.Extensions.DependencyInjection.Tests
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    using Builder;
    using Builder.Conventions;

    using Microsoft.Extensions.DependencyInjection;

    using TestServices;

    using Xunit;

    public class ServiceDescriptionsBuilderTests
    {
        [Fact]
        public void Builder_works_correct_even_when_there_is_any_assembly_definied()
        {
            // Arrange
            var builder = new ServiceDescriptorsBuilder();

            // Act
            builder.Build();
        }

        [Fact]
        public void When_Assembly_has_services_then_builder_should_describe_them_correct()
        {
            // Arrange
            var builder = CorrectBuilder();

            // Act
            var serviceDescriptions = builder.Build();

            // Assert
            Assert.Contains(
                CreateServiceDescriptor<ITestDependency, TestServiceDependency>(ServiceLifetime.Transient),
                serviceDescriptions,
                new ServiceDescriptorComparer());
        }

        [Fact]
        public void When_exists_service_with_RepleaceDependecy_then_builder_should_describe_only_this_one()
        {
            // Arrange
            var builder = CorrectBuilder();

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
        public void Builder_describe_generics_in_correct_way()
        {
            // Arrange
            var builder = CorrectBuilder();

            // Act
            var serviceDescriptions = builder.Build();

            // Assert
            Assert.Contains(
                CreateServiceDescriptor<IGeneric<int>, TestGenericService>(ServiceLifetime.Scoped),
                serviceDescriptions,
                new ServiceDescriptorComparer());
        }

        [Fact]
        public void Single_implementation_cannot_inherit_more_than_one_lifetime_scope()
        {
            // Arrange
            var builder = IncorrectBuilder();

            // Act & Assert
            var exception = Assert.Throws<DependencyDescriptionException>(() => builder.Build());
            Assert.Equal("Cannot set more than one lifetime", exception.Message);
        }

        [Fact]
        public void Builder_build_descriptors_from_assembly_in_less_that_10ms()
        {
            // Arrange
            var builder = CorrectBuilder();
            var attempts = 10000;
            var watch = new Stopwatch();
            var elapsedMilliseconds = 0L;

            // Act
            for (var i = 0; i < attempts; i++)
            {
                builder = CorrectBuilder();
                watch.Restart();
                builder.Build();
                watch.Stop();
                elapsedMilliseconds += watch.ElapsedMilliseconds;
            }

            // Assert
            Assert.True(10 >= elapsedMilliseconds / attempts);
        }

        private static Assembly GetTestAssembly()
        {
            return typeof(ITestDependency).GetTypeInfo().Assembly;
        }

        private static ServiceDescriptorsBuilder CorrectBuilder()
        {
            var assembly = GetTestAssembly();
            var builder = new ServiceDescriptorsBuilder().AddSourceAssembly(assembly);

            return builder.AddTypesConvention(new CorrectTestServicesConvention());
        }

        private static ServiceDescriptorsBuilder IncorrectBuilder()
        {
            var assembly = GetTestAssembly();
            var builder = new ServiceDescriptorsBuilder().AddSourceAssembly(assembly);

            return builder.AddTypesConvention(new IncorrectTestServicesConvention());
        }

        private static ServiceDescriptor CreateServiceDescriptor<TInterface, TImplementation>(ServiceLifetime lifetime)
        {
            return new ServiceDescriptor(typeof(TInterface), typeof(TImplementation), lifetime);
        }

        private class CorrectTestServicesConvention : ITypeSelectionConvention
        {
            public bool DoesPostSelect(Type type)
            {
                return true;
            }

            public virtual bool DoesSelect(Type type)
            {
                var @namespace = type.Namespace;
                var isCorrect = @namespace.EndsWith("Incorrect") == false && @namespace.Contains("TestServices");
                return isCorrect;
            }
        }

        private class IncorrectTestServicesConvention : CorrectTestServicesConvention
        {
            public override bool DoesSelect(Type type)
            {
                return base.DoesSelect(type) == false;
            }
        }
    }
}