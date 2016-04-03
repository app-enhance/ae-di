namespace AE.Extensions.DependencyInjection.Abstractions
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class DecorateServiceAttribute : RepleaceServiceAttribute
    {
        public DecorateServiceAttribute(Type repleacedType)
            : base(repleacedType)
        {
        }
    }
}