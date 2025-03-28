using System;
using System.Collections.Generic;
using System.Linq;

namespace KarenKrill.Common.Utilities
{
    public static class ReflectionUtilities
    {
        public static IEnumerable<Type> GetInheritorTypes(Type interfaceType, Type[] excludeTypes)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (assembly.FullName.StartsWith(nameof(KarenKrill)))
                {
                    var assemblyTypes = assembly.GetTypes();
                    foreach (var type in assemblyTypes)
                    {
                        if (interfaceType.IsAssignableFrom(type) && !excludeTypes.Contains(type))
                        {
                            yield return type;
                        }
                    }
                }
            }
        }
    }
}
