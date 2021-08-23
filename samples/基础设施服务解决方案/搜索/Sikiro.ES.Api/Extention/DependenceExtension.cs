using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Sikiro.Tookits.Interfaces;

namespace Sikiro.ES.Api.Extention
{
    public static class DependenceExtension
    {
        public static List<Type> GetTypeOfISerice(this AssemblyName assemblyName)
        {
            return AssemblyLoadContext.Default.LoadFromAssemblyName(assemblyName).ExportedTypes.Where(b => b.GetInterfaces().Contains(typeof(IDepend))).ToList();
        }

        public static void AddService(this IServiceCollection services)
        {
            var defaultAssemblyNames = DependencyContext.Default.GetDefaultAssemblyNames().Where(a => a.FullName.Contains("SF.")).ToList();

            var assemblies = defaultAssemblyNames.SelectMany(a => a.GetTypeOfISerice()).ToList();

            assemblies.ForEach(assembliy =>
            {
                services.AddScoped(assembliy);
            });
        }
    }
}
