namespace AE.Extensions.DependencyInjection.Abstractions
{
    using System;

    public class DependencyResolutionException : Exception
    {
        public DependencyResolutionException()
        {
        }

        public DependencyResolutionException(string message)
            : base(message)
        {
        }

        public DependencyResolutionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}