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
        public void Builder_throw_exception_if_give_null_conventions()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ServiceDescriptorsBuilder(null));
        }

        [Fact]
        public void Builder_works_correct_even_when_there_is_not_assembly_definied()
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
            var builder = Builder();

            // Act
            var serviceDescriptions = builder.Build();

            // Assert
            Assert.Contains(
                CreateServiceDescriptor<ITestDependency, TestServiceDependency>(ServiceLifetime.Transient),
                serviceDescriptions,
                new ServiceDescriptorComparer());
        }

        [Fact]
        public void When_add_custom_convention_builder_should_respect_it()
        {
            // Arrange
            var namespaceEndWith = "RespectConvention";
            var builder = Builder(namespaceEndWith);

            // Act
            var serviceDescriptions = builder.Build();

            // Assert
            Assert.All(serviceDescriptions, descriptor => Assert.EndsWith(namespaceEndWith, descriptor.ImplementationType.Namespace));
        }

        [Fact]
        public void When_exists_service_with_RepleaceDependecy_then_builder_should_describe_only_this_one()
        {
            // Arrange
            var builder = Builder();

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
        public void Cannot_mark_single_service_more_than_one_by_RepleaceDependency()
        {
            // Arrange
            var builder = Builder("MoreThanOneRepleaceDependency");

            // Act & Assert
            var exception = Assert.Throws<DependencyDescriptionException>(() => builder.Build());
            Assert.StartsWith("There is more than one service which override type", exception.Message);
        }

        [Fact]
        public void Builder_describe_generics_in_correct_way()
        {
            // Arrange
            var builder = Builder();

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
            var builder = Builder("MoreThanOneLifetime");

            // Act & Assert
            var exception = Assert.Throws<DependencyDescriptionException>(() => builder.Build());
            Assert.StartsWith("Cannot set more than one lifetime", exception.Message);
        }

        private static Assembly GetTestAssembly()
        {
            return typeof(ITestDependency).GetTypeInfo().Assembly;
        }

        private static ServiceDescriptorsBuilder Builder(string namespaceEndWith = "TestServices")
        {
            var assembly = GetTestAssembly();
            var builder = new ServiceDescriptorsBuilder().AddSourceAssembly(assembly);

            return builder.AddTypesConvention(new NamespaceEndWithTestServicesConvention(namespaceEndWith));
        }

        private static ServiceDescriptor CreateServiceDescriptor<TInterface, TImplementation>(ServiceLifetime lifetime)
        {
            return new ServiceDescriptor(typeof(TInterface), typeof(TImplementation), lifetime);
        }

        private class NamespaceEndWithTestServicesConvention : TypeSelectionConvention
        {
            private readonly string _namespaceEndWith;

            public NamespaceEndWithTestServicesConvention(string namespaceEndWith)
            {
                _namespaceEndWith = namespaceEndWith;
            }

            public override bool DoesSelect(Type type)
            {
                return type.Namespace.EndsWith(_namespaceEndWith);
            }
        }
    }
}
