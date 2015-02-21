using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class MappingActivityTestFixture
    {
        private readonly string m_schemaStoreId = Guid.NewGuid().ToString();

        [SetUp]
        public void Init()
        {
            var doc = new BinaryStore
            {
                Content = File.ReadAllBytes(@".\workflows\PemohonWakaf.xsd")
            };
            var store = new Mock<IBinaryStore>(MockBehavior.Strict);
            store.Setup(x => x.GetContent(m_schemaStoreId))
                .Returns(doc);
            ObjectBuilder.AddCacheList(store.Object);
        }


        [Test]
        public async Task Compile()
        {

            var wd = new WorkflowDefinition { Name = "Patient To Customer", Id = "mapping-patient-to-customer", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "mrn", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "patient", TypeName = "Bespoke." + ConfigurationManager.ApplicationName + "_patient.Domain.Patient" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "customer", TypeName = "Bespoke." + ConfigurationManager.ApplicationName + "_customer.Domain.Customer" });


            var mapping = new MappingActivity
            {
                Name = "MapPatientToCustomer",
                MappingDefinition = ConfigurationManager.ApplicationName + ".Integrations.Transforms.PatientToCustomer",
                OutputPath = "customer",
                WebId = "A",
                IsInitiator = true,
                NextActivityWebId = "End"
            };
            mapping.MappingSourceCollection.Add(new MappingSource
            {
                Variable = "patient"
            });
            wd.ActivityCollection.Add(mapping);


            wd.ActivityCollection.Add(new EndActivity { WebId = "End", Name = "EndWf" });


            var adapterPath = string.Format(@"C:\project\work\sph\bin\output\{0}.Customer.dll", ConfigurationManager.ApplicationName);
            var patientPath = string.Format(@"C:\project\work\sph\bin\output\{0}.Patient.dll", ConfigurationManager.ApplicationName);
            var mappingPath = string.Format(@"C:\project\work\sph\bin\output\{0}.PatientToCustomer.dll", ConfigurationManager.ApplicationName);

            File.Copy(adapterPath, AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".Customer.dll", true);
            File.Copy(patientPath, AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".Patient.dll", true);
            File.Copy(mappingPath, AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".PatientToCustomer.dll", true);
            var options = new CompilerOptions();
            options.AddReference(AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".Customer.dll");
            options.AddReference(AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".Patient.dll");
            options.AddReference(AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".PatientToCustomer.dll");
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\core.sph.dll"));
            options.AddReference(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll"));

            var cr = wd.Compile(options);
            cr.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(cr.Result);

            var wfDll = Assembly.LoadFile(cr.Output);
            dynamic wf = Activator.CreateInstance(wfDll.GetType("Bespoke.Sph.Workflows_PatientToCustomer_0.PatientToCustomerWorkflow"));


            wf.patient.Mrn = "784528";
            wf.patient.Gender = "Male";
            wf.patient.FullName = "Marco bin Pantani";
            wf.patient.Dob = new DateTime(1970, 2, 1);
            wf.patient.RegisteredDate = new DateTime(1995, 5, 21);



            Assert.AreNotEqual("Pantani", wf.staff.last_name);
            await wf.StartAsync();


            Assert.AreEqual("Marco", wf.staff.first_name.Trim(), JsonSerializerService.ToJsonString(wf.staff, true));
            Assert.AreEqual("Pantani", wf.staff.last_name.Trim(), JsonSerializerService.ToJsonString(wf.staff, true));
        }



    }
}