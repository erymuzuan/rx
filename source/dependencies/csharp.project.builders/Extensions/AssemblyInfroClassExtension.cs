using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Csharp.CompilersServices.Extensions
{
    public static class AssemblyInfroClassExtension
    {
        public static Class ToClass(this AssemblyInfoClass info)
        {
            return new Class(info.ToString()) { FileName = info.FileName };
        }
    }
}
