namespace AE.Extensions.DependencyInjection.Builder.Conventions
{
    using System;

    public abstract class TypeSelectionConvention : ITypeSelectionConvention
    {
        public virtual bool DoesSelect(Type type)
        {
            return true;
        }

        public virtual bool DoesPostSelect(Type type)
        {
            return true;
        }
    }
}