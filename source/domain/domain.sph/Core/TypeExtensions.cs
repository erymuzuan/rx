using System;
using Mono.Cecil;

namespace Bespoke.Sph.Domain
{
    public static class TypeExtensions
    {
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