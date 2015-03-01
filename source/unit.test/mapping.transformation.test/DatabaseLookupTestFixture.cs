using System;
using System.IO;
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
        public const string CONNECTION_STRING = "server=(localdb)\\Projects;database=his;trusted_connection=yes;";
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
        public async Task OneValue()
        {
            TransformDefinition td;
            var patient = CreatePatientMapping(out td, "__OneValue");

            var lookup = new SqlServerLookup
            {
                WebId = "lookup",
                Name = "lookup",
                SqlText = "SELECT [Income] FROM [dbo].[Patient] WHERE Mrn = @value1",
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
                WebId = "conn",
                Type = typeof(string),
                Value = CONNECTION_STRING
            };
            td.AddFunctoids(conn);
            lookup["connection"].Functoid = conn.WebId;

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = lookup.WebId,
                DestinationType = typeof(decimal),
                Destination = "Revenue"
            });

            var customer = await Compile(td, patient);
            Assert.IsNotNull(customer);
            Assert.AreEqual(5800m, customer.Revenue);

        }

        [TestMethod]
        public async Task WithConnectionStringFromConfig()
        {
            TransformDefinition td;
            var patient = CreatePatientMapping(out td, "__WithConnectionStringFromConfig");

            var lookup = new SqlServerLookup
            {
                WebId = "lookup",
                Name = "lookup",
                SqlText = "SELECT [Income] FROM [dbo].[Patient] WHERE Mrn = @value1",
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

            var conn = new ConfigurationSettingFunctoid
            {
                WebId = "conn",
                Section = "ConnectionString",
                Key = "His"
            };
            td.AddFunctoids(conn);
            lookup["connection"].Functoid = conn.WebId;

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = lookup.WebId,
                DestinationType = typeof(decimal),
                Destination = "Revenue"
            });

            var customer = await Compile(td, patient);
            Assert.IsNotNull(customer);
            Assert.AreEqual(5800m, customer.Revenue);

        }

        [TestMethod]
        public async Task TwoValues()
        {
            TransformDefinition td;
            var patient = CreatePatientMapping(out td, "__TwoValues");
            patient.Gender = "M";

            var lookup = new SqlServerLookup
            {
                WebId = "lookup",
                Name = "lookup",
                SqlText = "SELECT [Income] FROM [dbo].[Patient] WHERE [Mrn] = @value1 AND [Gender] = @value2",
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
            var gender = new SourceFunctoid
            {
                Name = "Gender",
                Field = "Gender",
                WebId = "Gender"
            };
            td.AddFunctoids(gender);
            lookup["value2"].Functoid = gender.WebId;

            var conn = new ConstantFunctoid
            {
                WebId = "conn",
                Type = typeof(string),
                Value = CONNECTION_STRING
            };
            td.AddFunctoids(conn);
            lookup["connection"].Functoid = conn.WebId;

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = lookup.WebId,
                DestinationType = typeof(decimal),
                Destination = "Revenue"
            });

            var customer = await Compile(td, patient);
            Assert.IsNotNull(customer);
            Assert.AreEqual(5800m, customer.Revenue);

        }

        private static async Task<dynamic> Compile(TransformDefinition td, dynamic patient)
        {
            byte[] buffer;
            using (var stream = new MemoryStream())
            {
                var options = new CompilerOptions(stream);
                var result = await td.CompileAsync(options);
                if (!result.Result)
                    result.Errors.ForEach(Console.WriteLine);
                Assert.IsTrue(result.Result, "Compiler fails");

                buffer = stream.GetBuffer();
            }


            var dll = Assembly.Load(buffer);
            var mt = dll.GetType(ConfigurationManager.ApplicationName + ".Integrations.Transforms." + td.Name);
            dynamic map = Activator.CreateInstance(mt);


            var customer = await map.TransformAsync(patient);
            return customer;
        }

        private static dynamic CreatePatientMapping(out TransformDefinition td, string name)
        {
            var patientType = Assembly.LoadFrom(PatientAssembly).GetType(PatientTypeName);
            var customerType = Assembly.LoadFrom(CustomerAssembly).GetType(CustomerTypeName);

            dynamic patient = Activator.CreateInstance(patientType);
            patient.FullName = "erymuzuan";
            patient.Mrn = "123";


            td = new TransformDefinition
            {
                Name = name,
                Description = "Just a description",
                InputType = patientType,
                OutputType = customerType
            };
            return patient;
        }
    }
}