namespace AE.Extensions.DependencyInjection.Tests
{
    using System.Collections.Generic;

    using Builder.Conventions;

    using TestServices.Abstract;

    using Xunit;

    public class InstanceClassTypeSelectionConventionTests
    {
        [Fact]
        public void Convention_select_any_default_class_type()
        {
            // Arrange
            var type = typeof(object);
            var convention = new InstanceClassTypeSelectionConvention();

            // Act
            var isSelected = convention.DoesSelect(type);

            // Assert
            Assert.True(isSelected);
        }

        [Fact]
        public void InstanceClass_convention_doesnt_select_abstract_class()
        {
            // Arrange
            var abstractType = typeof(AbstractClass);
            var convention = new InstanceClassTypeSelectionConvention();

            // Act
            var isNotSelected = convention.DoesSelect(abstractType);

            // Assert
            Assert.False(isNotSelected);
        }

        [Fact]
        public void InstanceClass_convention_doesnt_select_value_type()
        {
            // Arrange
            var abstractType = typeof(int);
            var convention = new InstanceClassTypeSelectionConvention();

            // Act
            var isNotSelected = convention.DoesSelect(abstractType);

            // Assert
            Assert.False(isNotSelected);
        }

        [Fact]
        public void InstanceClass_convention_doesnt_select_generic_definition_type()
        {
            // Arrange
            var abstractType = typeof(IEnumerable<>);
            var convention = new InstanceClassTypeSelectionConvention();

            // Act
            var isNotSelected = convention.DoesSelect(abstractType);

            // Assert
            Assert.False(isNotSelected);
        }
    }
}