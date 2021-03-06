﻿namespace AE.Extensions.DependencyInjection.Builder.Conventions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Abstractions;

    public class DependencyTypeSelectionConvention : TypeSelectionConvention
    {
        private readonly Type _dependencyType = typeof(IDependency);

        private readonly Type _notRegisterDependencyType = typeof(INotRegisterDependency);

        private readonly Dictionary<Type, Type> _repleacedDependencies;

        private readonly List<Type> _replecingOccured;

        public DependencyTypeSelectionConvention()
        {
            _repleacedDependencies = new Dictionary<Type, Type>();
            _replecingOccured = new List<Type>();
        }

        public override bool DoesSelect(Type type)
        {
            var doesSelect = _dependencyType.IsAssignableFrom(type) && !_notRegisterDependencyType.IsAssignableFrom(type);
            if (doesSelect)
            {
                MemorizeRepleacedDependencyIfAttached(type);
            }

            return doesSelect;
        }

        public override bool DoesPostSelect(Type type)
        {
            TryThrowExceptionWhenRepleaceDependencyOccurMoreThanOne(type);

            return _repleacedDependencies.Values.Contains(type) == false;
        }

        private void MemorizeRepleacedDependencyIfAttached(Type type)
        {
            var attribute = type.GetTypeInfo().GetCustomAttribute<RepleaceServiceAttribute>();
            if (attribute != null && _repleacedDependencies.ContainsKey(type) == false)
            {
                _repleacedDependencies.Add(type, attribute.RepleacedType);
            }
        }

        private void TryThrowExceptionWhenRepleaceDependencyOccurMoreThanOne(Type type)
        {
            if (_repleacedDependencies.ContainsKey(type))
            {
                var repleacedType = _repleacedDependencies[type];
                if (_replecingOccured.Any(t => t == repleacedType))
                {
                    throw new DependencyDescriptionException($"There is more than one service which override type: {repleacedType.Name}");
                }

                _replecingOccured.Add(repleacedType);
            }
        }
    }
}