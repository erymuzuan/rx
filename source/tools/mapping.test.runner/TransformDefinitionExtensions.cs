using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Bespoke.Sph.Domain;

namespace mapping.test.runner
{
    public static class TransformDefinitionExtensions
    {
        public static Type GetInputType(this TransformDefinition map)
        {
            var typeNames = map.InputTypeName.Split(',');
            var dll = typeNames.Last().Trim();
            var file = $"{ConfigurationManager.CompilerOutputPath}\\{dll}.dll";
            if (!File.Exists(file)) return null;

            var assembly = Assembly.LoadFrom(file);
            return assembly.GetType(typeNames.First());

        }
        public static Type GetOutputType(this TransformDefinition map)
        {
            var typeNames = map.OutputTypeName.Split(',');
            var dll = typeNames.Last().Trim();
            var file = $"{ConfigurationManager.CompilerOutputPath}\\{dll}.dll";
            if (!File.Exists(file)) return null;

            var assembly = Assembly.LoadFrom(file);
            return assembly.GetType(typeNames.First());

        }
        public static Type GetMapType(this TransformDefinition map)
        {
            var file = $"{ConfigurationManager.CompilerOutputPath}\\{map.AssemblyName}.dll";
            if (!File.Exists(file)) return null;

            var assembly = Assembly.LoadFrom(file);
            return assembly.GetType(map.FullTypeName);
        }
    }
}