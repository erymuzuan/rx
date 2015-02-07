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


            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\core.sph\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll"));


            var result = ed.Compile(options);
            result.Errors.ForEach(Console.WriteLine);
            DeployCustomEntity(ed);

            var dll = string.Format("{0}.{1}.dll", ConfigurationManager.ApplicationName, ed.Name);
            var assembly = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + @"\" + dll);
            var type = assembly.GetType("Bespoke.Dev_" + ed.Id + ".Domain." + ed.Name);
            return type;
        }


        private static void DeployCustomEntity(EntityDefinition ed)
        {
            var dll = string.Format("{0}.{1}.dll", ConfigurationManager.ApplicationName, ed.Name);
            var pdb = string.Format("{0}.{1}.pdb", ConfigurationManager.ApplicationName, ed.Name);
            var dllFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, dll);
            var pdbFullPath = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, pdb);

            File.Copy(dllFullPath, AppDomain.CurrentDomain.BaseDirectory + @"\" + dll, true);
            File.Copy(pdbFullPath, AppDomain.CurrentDomain.BaseDirectory + @"\" + pdb, true);

        }

    }
}
