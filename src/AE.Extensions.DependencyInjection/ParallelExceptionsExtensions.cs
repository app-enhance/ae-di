namespace AE.Extensions.DependencyInjection
{
    using System;
    using System.Linq;

    internal static class ParallelExceptionsExtensions
    {
        public static Exception FlattenAndCast<T>(this AggregateException e) where T : Exception, new()
        {
            var flattenException = e.Flatten();

            return TryBubbleUpException<T>(flattenException)
                   ?? CreateException<T>("Unhandled exceptions during paraller operations", flattenException);
        }

        private static Exception TryBubbleUpException<T>(AggregateException flattenException) where T : Exception, new()
        {
            if (flattenException.InnerExceptions.Count == 1)
            {
                var exception = flattenException.InnerExceptions.First();
                return exception is T ? exception : CreateException<T>(exception.Message, exception);
            }

            return null;
        }

        private static Exception CreateException<T>(params object[] parameters)
        {
            return Activator.CreateInstance(typeof(T), parameters) as Exception;
        }
    }
}