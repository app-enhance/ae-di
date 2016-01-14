namespace AE.Extensions.DependencyInjection
{
    using System;
    using System.Diagnostics;

    [AttributeUsage(AttributeTargets.Class)]
    public class RepleaceDependencyAttribute : Attribute
    {
        public RepleaceDependencyAttribute(Type repleacedType)
        {
            Debug.Assert(repleacedType != null);
            RepleacedType = repleacedType;
        }

        public Type RepleacedType { get; }
    }
}