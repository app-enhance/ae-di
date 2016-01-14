namespace AE.Extensions.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServicesDescriber
    {
        private static readonly Type DependencyType = typeof(IDependency);

        private static readonly Type ScopedDependencyType = typeof(IScopedDependency);

        private static readonly Type TransientDependencyType = typeof(ITransientDependency);

        private static readonly Type SingletonDependencyType = typeof(ISingletonDependency);

        private static readonly Type NotRegisterDependencyType = typeof(INotRegisterDependency);

        public static IEnumerable<ServiceDescriptor> DescribeFromAssemblies(params Assembly[] assembliesToScan)
        {
            var typesToRegistration = RetrieveTypesToRegistration(assembliesToScan);
            var serviceDescriptors = CreateDescriptors(typesToRegistration);

            return serviceDescriptors;
        }

        private static IEnumerable<Type> RetrieveTypesToRegistration(IEnumerable<Assembly> assembliesToScan)
        {
            var typesToRegistration = assembliesToScan.SelectMany(x => x.ExportedTypes).Where(IsTypeToAutoRegistration).ToList();
            RemoveRepleacedTypes(typesToRegistration);

            return typesToRegistration;
        }

        private static bool IsTypeToAutoRegistration(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return DependencyType.IsAssignableFrom(type) && !NotRegisterDependencyType.IsAssignableFrom(type) && typeInfo.IsClass
                   && !typeInfo.IsAbstract && !typeInfo.IsGenericTypeDefinition;
        }

        private static void RemoveRepleacedTypes(ICollection<Type> typesToRegistration)
        {
            var repleacedTypes = FindRepleacedTypes(typesToRegistration);
            foreach (var repleacedType in repleacedTypes)
            {
                typesToRegistration.Remove(repleacedType);
            }
        }

        private static IEnumerable<Type> FindRepleacedTypes(IEnumerable<Type> types)
        {
            var repleaceDependencyAttibutes = types.SelectMany(x => x.GetTypeInfo().GetCustomAttributes<RepleaceDependencyAttribute>());
            return repleaceDependencyAttibutes.Select(x => x.RepleacedType).ToList();
        }

        private static IEnumerable<ServiceDescriptor> CreateDescriptors(IEnumerable<Type> typesToRegistration)
        {
            foreach (var type in typesToRegistration)
            {
                var interfaces = type.GetInterfaces();
                var scope = RetieveScope(interfaces);

                foreach (var @interface in interfaces)
                {
                    yield return new ServiceDescriptor(@interface, type, scope);
                }

                yield return new ServiceDescriptor(type, type, scope);
            }
        }

        private static ServiceLifetime RetieveScope(IEnumerable<Type> interfaces)
        {
            var scope = ValidateDependencyScope(interfaces);
            if (scope == SingletonDependencyType)
            {
                return ServiceLifetime.Singleton;
            }

            if (scope == ScopedDependencyType)
            {
                return ServiceLifetime.Scoped;
            }

            if (scope == TransientDependencyType)
            {
                return ServiceLifetime.Transient;
            }

            throw new DependencyDescriptionException("Cannot retrieve scope");
        }

        private static Type ValidateDependencyScope(IEnumerable<Type> interfaces)
        {
            var validScopes = interfaces.Where(x => x == ScopedDependencyType || x == TransientDependencyType || x == SingletonDependencyType);
            if (validScopes.Any() == false)
            {
                throw new DependencyDescriptionException("There is no defined lifetime");
            }

            if (validScopes.Count() > 1)
            {
                throw new DependencyDescriptionException("Cannot set more than one lifetime");
            }

            return validScopes.First();
        }
    }
}