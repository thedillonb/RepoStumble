using System;
using System.Linq;
using Splat;
using System.Collections.Generic;
using System.Diagnostics;

namespace RepositoryStumble.Core.Services
{
    public interface IServiceConstructor
    {
        object Construct(Type type);
    }

    public class ServiceConstructor : IServiceConstructor
    {
        public object Construct(Type type)
        {
            var constructor = type.GetConstructors().First();
            var parameters = constructor.GetParameters();
            var args = new List<object>(parameters.Length);
            foreach (var p in parameters)
            {
                var argument = Locator.Current.GetService(p.ParameterType);
                if (argument == null)
                    Debugger.Break();
                args.Add(argument);
            }
            return Activator.CreateInstance(type, args.ToArray(), null);
        }
    }

    public static class ServiceConstructorExtensions
    {
        public static T Construct<T>(this IServiceConstructor serviceConstructor)
        {
            return (T)serviceConstructor.Construct(typeof(T));
        }
    }
}

