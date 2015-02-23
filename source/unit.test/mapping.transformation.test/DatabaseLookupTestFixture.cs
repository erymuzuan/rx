using System;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mapping.transformation.test
{
    [TestClass]
    public class DatabaseLookupTestFixture
    {
        public const string CUSTOMER = "Customer";
        public const string PATIENT = "Patient";
        public readonly static string PatientAssembly = @".\" + ConfigurationManager.ApplicationName + ".Patient.dll";
        public readonly static string CustomerAssembly = @".\" + ConfigurationManager.ApplicationName + ".Customer.dll";
        public readonly static string CustomerTypeName = "Bespoke." + ConfigurationManager.ApplicationName + "_customer.Domain.Customer";
        public readonly static string PatientTypeName = "Bespoke." + ConfigurationManager.ApplicationName + "_patient.Domain.Patient";

        [TestInitialize]
        public void Setup()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
        }

        [TestMethod]
        public async Task One()
        {
            var patientType = Assembly.LoadFrom(PatientAssembly).GetType(PatientTypeName);
            var customerType = Assembly.LoadFrom(CustomerAssembly).GetType(CustomerTypeName);

            dynamic patient = Activator.CreateInstance(patientType);
            patient.FullName = "erymuzuan";
            patient.Mrn = "123";


            var td = new TransformDefinition
            {
                Name = "__DatabaseLookup",
                Description = "Just a description",
                InputType = patientType,
                OutputType = customerType
            };

            var lookup = new SqlServerLookup
            {
                WebId = "lookup",
                Name = "lookup",
                Table = "Patient",
                Schema = "dbo",
                Column = "Income",
                Predicate = "Mrn = '{0}'",
                DefaultValue = "decimal.Zero",
                OutputTypeName = typeof(decimal).GetShortAssemblyQualifiedName()
            };
            lookup.Initialize();
            td.AddFunctoids(lookup);

            var mrn = new SourceFunctoid
            {
                Name = "Mrn",
                Field = "Mrn",
                WebId = "Mrn"
            };
            td.AddFunctoids(mrn);
            lookup["value1"].Functoid = mrn.WebId;

            var conn = new ConstantFunctoid
            {
                WebId = "conn" ,
                Type = typeof(string),
                Value = "server=(localdb)\\Projects;database=his;trusted_connection=yes;"
            };
            td.AddFunctoids(conn);
            lookup["connection"].Functoid = conn.WebId;

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = lookup.WebId,
                DestinationType = typeof(decimal),
                Destination = "Revenue"
            });

            var options = new CompilerOptions();
            var codes = td.GenerateCode();
            var sources = td.SaveSources(codes);
            var result = await td.CompileAsync(options, sources);
            if (!result.Result)
                result.Errors.ForEach(Console.WriteLine);

            Assert.IsTrue(result.Result, "Compiler fails");
            var dll = Assembly.LoadFile(result.Output);
            var mt = dll.GetType(ConfigurationManager.ApplicationName + ".Integrations.Transforms." + td.Name);
            dynamic map = Activator.CreateInstance(mt);


            var output = await map.TransformAsync(patient);
            Assert.IsNotNull(output);
            Assert.AreEqual(5800m, output.Revenue);



        }
    }
}