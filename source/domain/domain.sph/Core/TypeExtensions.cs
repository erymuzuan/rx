using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;

namespace Bespoke.Sph.Domain
{
    public static class TypeExtensions
    {
        public static bool HasInterface(this TypeDefinition ct, Type interfaceType)
        {
            while (null != ct)
            {
                var hasInterface = ct.Interfaces.Any(x => x.FullName == interfaceType.FullName);
                if (hasInterface) return true;
                ct = ct.BaseType.LoadTypeDefinition();
            }
            return false;

        }
        public static IEnumerable<PropertyDefinition> LoadProperties(this TypeDefinition currentType)
        {
            var properties = new List<PropertyDefinition>();
            while (null != currentType)
            {
                properties.AddRange(currentType.Properties);
                currentType = currentType.BaseType.LoadTypeDefinition();
            }
            return properties;
        }

        public static TypeDefinition LoadTypeDefinition(this TypeReference type)
        {
            if (type == null) return null;
            if (type.FullName == typeof(object).FullName) return null;
            var scope = type.Scope;
            var name = scope.Name.EndsWith(".dll") ? scope.Name : scope.Name + ".dll";
            var probingPaths = new[]
                          {
                                $@"{ConfigurationManager.CompilerOutputPath}\{name}",
                                $@"{ConfigurationManager.WebPath}\bin\{name}",
                                $@"{ConfigurationManager.SubscriberPath}\{name}"
                            };
            var ca = probingPaths
                .Where(File.Exists)
                .Select(AssemblyDefinition.ReadAssembly)
                .FirstOrDefault();

            if (null == ca)
            {
                var tt = $"{type.Namespace}.{type.Name}, {scope}";
                var clrType = Type.GetType(tt);
                if (null != clrType)
                {
                    ca = AssemblyDefinition.ReadAssembly(clrType.Assembly.Location);
                }
            }

            if (null == ca)
                throw new FileNotFoundException($"Cannot fild {name} in web\\bin, output, subscribers or GAC when looking for {type.FullName} ");
            var generic = type as GenericInstanceType;
            if (null != generic)
            {
                var elementType = generic.ElementType.FullName;
                return ca.MainModule.Types.SingleOrDefault(x => x.FullName == elementType);
            }
            return ca.MainModule.Types.SingleOrDefault(x => x.FullName == type.FullName);
        }

        public static bool IsOfType(this TypeReference tr, Type type)
        {
            if (null == tr) return false;
            var tr0 = type.GetTypeReference();
            if (null == tr0) return false;
            return tr0.FullName == tr.FullName;
        }
        public static bool IsOfType(this Type type, TypeReference tr)
        {
            var tr0 = type.GetTypeReference();
            return tr0.FullName == tr.FullName;
        }
        public static TypeReference GetTypeReference(this Type type)
        {
            var dll = type.Assembly.Location;
            var tr = new TypeReference(type.Namespace, type.Name, ModuleDefinition.ReadModule(dll), null);
            return tr;
        }
    }
}