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
