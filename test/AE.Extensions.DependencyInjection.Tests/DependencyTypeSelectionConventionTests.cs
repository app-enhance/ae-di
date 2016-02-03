namespace AE.Extensions.DependencyInjection.Tests
{
    using Builder.Conventions;
    using Builder;

    using TestServices;
    using TestServices.MoreThanOneRepleaceDependency;

    using Xunit;

    public class DependencyTypeSelectionConventionTests
    {
        [Fact]
        public void Dependency_convention_select_only_class_which_ineriht_IDependency()
        {
            // Arrange
            var withIDependency = typeof(TestServiceDependency);
            var withoutIDependency = typeof(object);
            var convention = new DependencyTypeSelectionConvention();

            // Act
            var isSelected = convention.DoesSelect(withIDependency);
            var isNotSelected = convention.DoesSelect(withoutIDependency);

            // Assert
            Assert.True(isSelected);
            Assert.False(isNotSelected);
        }

        [Fact]
        public void When_class_ineriht_INotRegisterDependency_then_is_not_selected_even_if_ineriht_IDependency()
        {
            // Arrange
            var notRegisterClass = typeof(NotRegisterDependency);
            var convention = new DependencyTypeSelectionConvention();

            // Act
            var isNotSelected = convention.DoesSelect(notRegisterClass);

            // Assert
            Assert.False(isNotSelected);
        }

        [Fact]
        public void Cannot_scan_every_type_many_times_even_with_RepleaceDependencyAttribute()
        {
            // Arrange
            var defaultType = typeof(TestServiceDependency);
            var typeWithRepleaceDependency = typeof(RepleacingTestService);
            var convention = new DependencyTypeSelectionConvention();

            // Act
            convention.DoesSelect(defaultType);
            convention.DoesSelect(defaultType);
            convention.DoesSelect(typeWithRepleaceDependency);
            convention.DoesSelect(typeWithRepleaceDependency);
        }

        [Fact]
        public void Dependency_convention_does_not_post_scan_repleaced_dependencies_when_they_scaned_before()
        {
            // Arrange
            var repleacedDependency = typeof(RepleacedTestService);
            var repleacingDependency = typeof(RepleacingTestService);
            var convention = new DependencyTypeSelectionConvention();
            convention.DoesSelect(repleacingDependency);

            // Act
            var isNotSelected = convention.DoesPostSelect(repleacedDependency);

            // Assert
            Assert.False(isNotSelected);
        }

        [Fact] // Consider if throw exception when type is not scaned before post scan
        public void Dependency_convention_throw_exception_when_post_scan_more_than_one_type_which_repleace_the_same_dependency()
        {
            // Arrange
            var firstRepleacingType = typeof(FirstServiceWhichRepleace);
            var secondRepleacingType = typeof(SecondServiceWhichRepleace);
            var convention = new DependencyTypeSelectionConvention();
            convention.DoesSelect(firstRepleacingType);
            convention.DoesSelect(secondRepleacingType);

            // Act & Assert
            var exception = Assert.Throws<DependencyDescriptionException>(
                () =>
                    {
                        convention.DoesPostSelect(firstRepleacingType);
                        convention.DoesPostSelect(secondRepleacingType);
                    });
            Assert.StartsWith("There is more than one service which override type", exception.Message);
        }
    }
}