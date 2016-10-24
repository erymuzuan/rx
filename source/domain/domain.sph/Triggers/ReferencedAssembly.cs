using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public static class ReferencedAssemblyExtension
    {
        public static ReferencedAssembly Add(this IList<ReferencedAssembly> list, string name)
        {
            var location = $"{ConfigurationManager.WebPath}\\bin\\{name}.dll";
            if(!File.Exists(location))
                location = $"{ConfigurationManager.CompilerOutputPath}\\{name}.dll";

            var package = new ReferencedAssembly
            {
                Name = name,
                FullName = Path.GetFullPath(location),
                Location = location
            };
            list.Add(package);
            return package;
        }
        public static ReferencedAssembly Add<T>(this IList<ReferencedAssembly> list)
        {
            var package = new ReferencedAssembly
            {
                Name = typeof(T).Assembly.FullName,
                FullName = typeof(T).Assembly.FullName,
                Location = typeof(T).Assembly.Location
            };
            list.Add(package);
            return package;
        }
        public static ReferencedAssembly AddPackage(this IList<ReferencedAssembly> list, string name, string version = "", string framework = "net45", string assemblyName = "")
        {
            var package = ConfigurationManager.GetPackage(name, version, framework, assemblyName);
            if (list.All(x => x.Name != name))
                list.Add(package);

            return package;
        }


    }
    public partial class ReferencedAssembly : DomainObject
    {
        public string GetAssemblyLocation()
        {
            var file = Path.GetFileName(this.Location);

            var output = $"{ConfigurationManager.CompilerOutputPath}\\{file}";
            if (File.Exists(output))
                return output;

            var web = $"{ConfigurationManager.WebPath}\\bin\\{file}";
            if (File.Exists(web))
                return web;

            if (File.Exists(this.Location))
                return this.Location;

            throw new Exception($"Cannot find any '{file}'");
        }
    }
}