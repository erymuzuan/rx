using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Amido.NAuto;
using Amido.NAuto.Randomizers;
using Bespoke.Sph.Domain;

namespace domain.test
{
    public class CustomerEntityHelper
    {
        public static dynamic CreateCustomerInstance(Type type)
        {
            dynamic customer = Activator.CreateInstance(type);
            customer.FullName = NAuto.GetRandomString(8, 18, CharacterSetType.Alpha, Spaces.Middle);
            customer.Age = NAuto.GetRandomInteger(25, 65);
            customer.Gender = new[] { "Male", "Female" }.OrderBy(f => Guid.NewGuid()).First();
            customer.Address.State =
                new[] { "Kelantan", "Selangor", "Perak", "Kuala Lumpur", "Johor", "Melaka", "Negeri Sembilan" }.OrderBy(
                    f => Guid.NewGuid()).First();
            customer.Address.Locality = new[] { "Rural", "Urban", "Surburb" }.OrderBy(f => Guid.NewGuid()).First();
            customer.RegisteredDate = DateTime.Today.AddDays(-NAuto.GetRandomInteger(50, 500));

            customer.IsPriority = customer.FullName.Length % 2 == 0;
            customer.Contact.Name = NAuto.GetRandomString(8, 18, CharacterSetType.Alpha, Spaces.Middle);
            customer.Address.Street1 = NAuto.GetRandomString(8, 18, CharacterSetType.Alpha, Spaces.Middle);
            return customer;
        }
        public static dynamic CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public static Type CompileEntityDefinition(EntityDefinition ed)
        {
            var options = new CompilerOptions
            {
                IsVerbose = false,
                IsDebug = true
            };


            options.ReferencedAssembliesLocation.Add(Path.GetFullPath($@"{ConfigurationManager.WebPath}\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath($@"{ConfigurationManager.WebPath}\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath($@"{ConfigurationManager.WebPath}\bin\Newtonsoft.Json.dll"));


            var codes = ed.GenerateCode();
            var sources = ed.SaveSources(codes);
            var result = ed.Compile(options, sources);
            result.Errors.ForEach(Console.WriteLine);
            DeployCustomEntity(ed);

            var dll = $"{ConfigurationManager.ApplicationName}.{ed.Name}.dll";
            var assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + @"\" + dll);
            var type = assembly.GetType(ed.TypeName);
            return type;
        }


        private static void DeployCustomEntity(EntityDefinition ed)
        {
            var dll = $"{ConfigurationManager.ApplicationName}.{ed.Name}.dll";
            var pdb = $"{ConfigurationManager.ApplicationName}.{ed.Name}.pdb";
            var dllFullPath = Path.Combine(ConfigurationManager.CompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.CompilerOutputPath, pdb);

            File.Copy(dllFullPath, AppDomain.CurrentDomain.BaseDirectory + @"\" + dll, true);
            File.Copy(pdbFullPath, AppDomain.CurrentDomain.BaseDirectory + @"\" + pdb, true);

        }

    }
}
