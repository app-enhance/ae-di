namespace AE.Extensions.DependencyInjection
{
    using System;
    using System.Linq;

    internal static class ParallelExceptionsExtensions
    {
        public static T FlattenAndCast<T>(this AggregateException e) where T : Exception, new()
        {
            var flattenException = e.Flatten();
            if (flattenException.InnerExceptions.Count == 1)
            {
                return Activator.CreateInstance(typeof(T), new[] { flattenException.InnerExceptions.First().Message }) as T;
            }

            return Activator.CreateInstance(typeof(T), new object[] { "Unhandled exceptions during descrive service type", flattenException }) as T;
        }
    }
}