using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppCore.DependencyInjection.Microsoft.Extensions
{
    internal class AssemblyScanner
    {
        public IEnumerable<Type> GetTypes(Assembly assembly, Type serviceType)
        {
            IEnumerable<Type> exportedTypes = assembly.ExportedTypes.Where(
                t =>
                {
                    TypeInfo ti = t.GetTypeInfo();
                    return ti.IsClass
                           && !ti.IsAbstract
                           && ti.DeclaredConstructors.Any(
                               ci => ci.IsPublic && !ci
                                         .IsStatic);
                });

            return exportedTypes.Where(t =>
            {
                IEnumerable<Type> assignableTypes = t.GetTypesAssignableFrom();

                assignableTypes = assignableTypes.Concat(
                    assignableTypes.Where(
                                       t2 => t2.IsConstructedGenericType)
                                   .Select(t2 => t2.GetGenericTypeDefinition()));
                
                return assignableTypes
                    .Contains(serviceType);
            });
        }
    }
}
