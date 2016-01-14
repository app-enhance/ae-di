namespace AE.Extensions.DependencyInjection
{
    using System;

    public static class ServiceProviderExtensions
    {
        public static T Resolve<T>(this IServiceProvider serviceProvider) where T : class
        {
            var service = serviceProvider.GetService(typeof(T)) as T;
            if (service == null)
            {
                throw new DependencyResolutionException($"The requested service '{nameof(T)}' has not been registered.");
            }

            return service;
        }
    }
}