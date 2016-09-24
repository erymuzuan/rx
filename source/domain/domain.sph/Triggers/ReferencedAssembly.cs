using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    public static class ReferencedAssemblyExtension
    {
        public static ReferencedAssembly AddPackage(this IList<ReferencedAssembly> list, string name, string version = "", string framework = "net45")
        {
            var package = ConfigurationManager.GetPackage(name, version, framework);
            if (list.All(x => x.Name != name))
                list.Add(package);

            return package;
        }
    }
    public partial class ReferencedAssembly : DomainObject
    {

    }
}