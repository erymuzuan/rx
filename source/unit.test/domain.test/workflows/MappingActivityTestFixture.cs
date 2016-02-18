using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;
using Xunit;

namespace domain.test.workflows
{
    public class MappingActivityTestFixture
    {
        private readonly string m_schemaStoreId = Guid.NewGuid().ToString();
        
        public MappingActivityTestFixture()
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


        [Fact]
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


            var adapterPath = $@"{ConfigurationManager.CompilerOutputPath}\{ConfigurationManager.ApplicationName}.Customer.dll";
            var patientPath = $@"{ConfigurationManager.CompilerOutputPath}\{ConfigurationManager.ApplicationName}.Patient.dll";
            var mappingPath =$@"{ConfigurationManager.CompilerOutputPath}\{ConfigurationManager.ApplicationName}.PatientToCustomer.dll";

            File.Copy(adapterPath, AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".Customer.dll", true);
            File.Copy(patientPath, AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".Patient.dll", true);
            File.Copy(mappingPath, AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".PatientToCustomer.dll", true);
            var options = new CompilerOptions();
            options.AddReference(AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".Customer.dll");
            options.AddReference(AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".Patient.dll");
            options.AddReference(AppDomain.CurrentDomain.BaseDirectory + @"\" + ConfigurationManager.ApplicationName + ".PatientToCustomer.dll");
      
            var cr = wd.Compile(options);
            cr.Errors.ForEach(Console.WriteLine);
            Assert.True(cr.Result, cr.ToString());

            var wfDll = Assembly.LoadFile(cr.Output);
            dynamic wf = Activator.CreateInstance(wfDll.GetType("Bespoke.Sph.Workflows_PatientToCustomer_0.PatientToCustomerWorkflow"));


            wf.patient.Mrn = "784528";
            wf.patient.Gender = "Male";
            wf.patient.FullName = "Marco bin Pantani";
            wf.patient.Dob = new DateTime(1970, 2, 1);
            wf.patient.RegisteredDate = new DateTime(1995, 5, 21);



            Assert.NotEqual("Pantani", wf.staff.last_name);
            await wf.StartAsync();


            Assert.Equal("Marco", wf.staff.first_name.Trim(), JsonSerializerService.ToJsonString(wf.staff, true));
            Assert.Equal("Pantani", wf.staff.last_name.Trim(), JsonSerializerService.ToJsonString(wf.staff, true));
        }



    }
}