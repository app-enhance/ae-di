namespace AE.Extensions.DependencyInjection.Builder.Conventions
{
    using System;

    public interface ITypeSelectionConvention : ITypeSelector
    {
        bool DoesPostSelect(Type type);
    }
}