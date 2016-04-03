namespace AE.Extensions.DependencyInjection.Abstractions
{
    using System;
    using System.Diagnostics;

    [AttributeUsage(AttributeTargets.Class)]
    public class RepleaceServiceAttribute : Attribute
    {
        public RepleaceServiceAttribute(Type repleacedType)
        {
            Debug.Assert(repleacedType != null);
            RepleacedType = repleacedType;
        }

        public Type RepleacedType { get; }
    }
}