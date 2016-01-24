namespace AE.Extensions.DependencyInjection
{
    using System;
    using System.Linq;

    internal static class ParallelExceptionsExtensions
    {
        public static Exception FlattenTryBubbleUp(this AggregateException e)
        {
            var flattenException = e.Flatten();
            return TryBubbleUpException(flattenException) ?? flattenException;
        }

        private static Exception TryBubbleUpException(AggregateException flattenException)
        {
            return flattenException.InnerExceptions.Count == 1 ? flattenException.InnerExceptions.First() : null;
        }
    }
}