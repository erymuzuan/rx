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
    public class MultipleInputTestFixture
    {
        [TestInitialize]
        public void Setup()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
        }


        private dynamic CreateEntityInstance(string typeName)
        {
            var name = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, typeName.ToLower(), typeName);
            var assemblyFile = string.Format(@".\{0}.{1}.dll", ConfigurationManager.ApplicationName, typeName);
            var type = Assembly.LoadFrom(assemblyFile).GetType(name);
            dynamic instance = Activator.CreateInstance(type);
            return instance;
        }


        [TestMethod]
        public async Task PatientDistrictAndStateToCustomer()
        {
            var patient = this.CreateEntityInstance("Patient");
            patient.FullName = "Erymuzuan Mustapa";
            patient.Mrn = "ABC123";
            patient.CreatedDate = new DateTime(2000, 1, 1);

            var district = this.CreateEntityInstance("District");
            district.Name = "Tanah Merah";

            var state = this.CreateEntityInstance("State");
            state.Name = "Kelantan";


            var patientAssembly = @".\" + ConfigurationManager.ApplicationName + ".Customer.dll";
            var patientTypeName = "Bespoke." + ConfigurationManager.ApplicationName + "_customer.Domain.Customer";
            var customerType = Assembly.LoadFrom(patientAssembly).GetType(patientTypeName);


            var td = new TransformDefinition
            {
                Name = "__EmployeeMapping",
                Description = "Just a description",
                OutputType = customerType
            };
            td.InputCollection.Add(new MethodArg { Name = "Patient", Type = patient.GetType() });
            td.InputCollection.Add(new MethodArg { Name = "District", Type = district.GetType() });
            td.InputCollection.Add(new MethodArg { Name = "State", Type = state.GetType() });

            td.FunctoidCollection.Add(new ConstantFunctoid
            {
                Value = "2011-05-05",
                Type = typeof(string),
                WebId = "2011-05-05"
            });
            td.FunctoidCollection.Add(new ConstantFunctoid
            {
                Value = "yyyy-MM-dd",
                Type = typeof(string),
                WebId = "yyyy-MM-dd"
            });

            var df = new ParseDateTimeFunctoid { WebId = "df" };
            df.Initialize();
            df["format"].Functoid = "yyyy-MM-dd";
            df["value"].Functoid = "2011-05-05";

            td.FunctoidCollection.Add(df);
            td.MapCollection.Add(new DirectMap
            {
                Source = "Patient.Mrn",
                Type = typeof(int),
                Destination = "Id"
            });


            var genderFnctd = new ScriptFunctoid
            {
                Expression = "return item.Patient.Gender == \"Male\"? \"Lelaki\" : \"Perempuan\";",
                Name = "genderFunct",
                WebId = "genderFunct",
                OutputType = typeof(string)

            };
            td.FunctoidCollection.Add(genderFnctd);

            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = genderFnctd.WebId,
                Destination = "Contact.Email",
                DestinationType = typeof(string)
            });
            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = df.WebId,
                Destination = "RegisteredDate",
                DestinationType = typeof(DateTime)
            });
            const string FORMATTING_FUNCTOID = "formattingFunctoid";
            var ff = new FormattingFunctoid
            {
                Format = "Phone from date {0:yyyy-MM-dd}",
                WebId = FORMATTING_FUNCTOID
            };
            ff.Initialize();
            var sfff = new SourceFunctoid { Field = "Patient.CreatedDate", WebId = Guid.NewGuid().ToString() };
            ff["value"].Functoid = sfff.WebId;
            td.FunctoidCollection.Add(sfff);
            td.FunctoidCollection.Add(ff);
            td.MapCollection.Add(new FunctoidMap
            {
                Functoid = FORMATTING_FUNCTOID,
                Destination = "Contact.Telephone",
                DestinationType = typeof(string)
            });

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


            var output = await map.TransformAsync(patient, district, state);
            Assert.IsNotNull(output);
            Assert.AreEqual(patient.Mrn, output.Id);
            Assert.AreEqual(new DateTime(2011, 5, 5), output.RegisteredDate);
            Assert.AreEqual(output.Contact.Telephone, "Phone from date 2000-01-01", output.Contact.Telephone);
        }

    }
}