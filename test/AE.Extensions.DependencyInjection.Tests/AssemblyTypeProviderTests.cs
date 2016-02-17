namespace AE.Extensions.DependencyInjection.Tests
{
    using System;
    using System.Reflection;

    using Builder;
    using Builder.Conventions;

    using TestServices;

    using Xunit;

    public class AssemblyTypeProviderTests
    {
        [Fact]
        public void AssemblyTypeProvider_throw_exception_when_assembly_is_null()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AssemblyTypeProvider(null));
        }

        [Fact]
        public void When_selector_is_null_return_all_types()
        {
            // Arrange
            var assembly = GetTestAssembly();
            var typeProvider = new AssemblyTypeProvider(assembly);

            // Act
            var types = typeProvider.RetrieveTypes(null);

            // Assert
            Assert.Equal(assembly.ExportedTypes, types);
        }

        [Fact]
        public void AssemblyTypeProvider_use_typeSelector_in_correct_way()
        {
            // Arrange
            var assembly = GetTestAssembly();
            var selector = new TypeNamesStartsWithSelector("S");
            var typeProvider = new AssemblyTypeProvider(assembly);

            // Act
            var types = typeProvider.RetrieveTypes(selector);

            // Assert
            Assert.All(types, type => type.Name.StartsWith("S"));
        }

        private static Assembly GetTestAssembly()
        {
            return typeof(ITestDependency).GetTypeInfo().Assembly;
        }

        private class TypeNamesStartsWithSelector : ITypeSelector
        {
            private readonly string _startWith;

            public TypeNamesStartsWithSelector(string startWith)
            {
                _startWith = startWith;
            }

            public bool DoesSelect(Type type)
            {
                return type.Name.StartsWith(_startWith);
            }
        }
    }
}