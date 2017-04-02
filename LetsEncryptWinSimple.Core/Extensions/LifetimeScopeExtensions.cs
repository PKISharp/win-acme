﻿using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Reflection;
using LetsEncryptWinSimple.Core.Interfaces;

namespace LetsEncryptWinSimple.Core.Extensions
{
    public static class LifetimeScopeExtensions
    {
        public static Dictionary<string, IPlugin> GetImplementingTypes<T>(this ILifetimeScope scope)
        {
            //base on http://bendetat.com/autofac-get-registration-types.html article
            var types = scope.ComponentRegistry
                .RegistrationsFor(new TypedService(typeof(T)))
                .Select(x => x.Activator)
                .OfType<ReflectionActivator>()
                .Select(x => x.LimitType);

            var plugins = new Dictionary<string, IPlugin>();

            foreach (var type in types)
            {
                var plugin = scope.Resolve(type) as IPlugin;
                if (plugin != null)
                    plugins.Add(plugin.Name, plugin);
            }

            return plugins;
        }
    }
}