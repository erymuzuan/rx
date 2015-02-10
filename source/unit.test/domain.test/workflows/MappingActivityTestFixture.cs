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

            var wd = new WorkflowDefinition { Name = "Mapping patient into MySql employee", Id = "patient-mysql-insert-employee", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "empNo", Type = typeof(int) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "patient", TypeName = "Bespoke.Dev_patient.Domain.Patient" });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "staff", TypeName = "Dev.Adapters.employees.employees" });


            var mapping = new MappingActivity
            {
                Name = "MapPatientToEmployee",
                MappingDefinition = "Dev.Integrations.Transforms.PatientToMySqlEmployee001",
                OutputPath = "staff",
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

            
            const string ADAPTER_PATH = @"C:\project\work\sph\bin\output\Dev.__MySqlTestAdapter.dll";
            const string PATIENT_PATH = @"C:\project\work\sph\bin\output\Dev.Patient.dll";
            const string MAPPING_PATH = @"C:\project\work\sph\bin\output\Dev.PatientToMySqlEmployee001.dll";

            File.Copy(ADAPTER_PATH, AppDomain.CurrentDomain.BaseDirectory + @"\Dev.__MySqlTestAdapter.dll", true);
            File.Copy(PATIENT_PATH, AppDomain.CurrentDomain.BaseDirectory + @"\Dev.Patient.dll", true);
            File.Copy(MAPPING_PATH, AppDomain.CurrentDomain.BaseDirectory + @"\Dev.PatientToMySqlEmployee001.dll", true);
            var options = new CompilerOptions();


            var cr = await wd.CompileAsync(options).ConfigureAwait(false);
            cr.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(cr.Result);

            var wfDll = Assembly.Load(cr.Buffer);
            dynamic wf = Activator.CreateInstance(wfDll.GetType("Bespoke.Sph.Workflows_PatientMysqlInsertEmployee_0.PatientMysqlInsertEmployeeWorkflow"));


            wf.patient.Mrn = "784528";
            wf.patient.Gender = "Male";
            wf.patient.FullName = "Marco bin Pantani";
            wf.patient.Dob = new DateTime(1970, 2, 1);
            wf.patient.RegisteredDate = new DateTime(1995, 5, 21);



            Assert.AreNotEqual("Pantani",wf.staff.last_name);
            await wf.StartAsync();
            

            Assert.AreEqual("Marco", wf.staff.first_name.Trim(), JsonSerializerService.ToJsonString(wf.staff, true));
            Assert.AreEqual("Pantani", wf.staff.last_name.Trim(), JsonSerializerService.ToJsonString(wf.staff, true));
        }



    }
}