using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.workflows
{
    [Trait("Category", "Workflow")]
    public class ReceiveActivityTestFixture
    {
        private readonly ITestOutputHelper m_helper;
        private readonly string m_schemaStoreId = Guid.NewGuid().ToString();

        public ReceiveActivityTestFixture(ITestOutputHelper helper)
        {
            m_helper = helper;
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
        public async Task ReceiveAndInitiate()
        {
            string patientTypeFullName = $"Bespoke.{ConfigurationManager.ApplicationName}.Patients.Domain.Patient, {ConfigurationManager.ApplicationName}.Patient";
            var wd = new WorkflowDefinition { Name = "Receive Register new patient", Id = "receive-register-patient", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "mrn", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ClrTypeVariable { Name = "patient", TypeName = patientTypeFullName , CanInitiateWithDefaultConstructor = true});


            var location = $"{AppDomain.CurrentDomain.BaseDirectory}\\{ConfigurationManager.ApplicationName}.Patient.dll";
            wd.ReferencedAssemblyCollection.Add(new ReferencedAssembly
            {
                Name = "Patient",
                Location = location
            });

            var regiterStaff = new ReceiveActivity
            {
                Name = "RegisterPatient",
                Operation = "Register",
                MessagePath = "patient",
                NextActivityWebId = "B",
                IsInitiator = true,
                WebId = "A"
            };
            wd.ActivityCollection.Add(regiterStaff);

            // var code = regiterStaff.GenerateExecMethodBody(wd);
            // Assert.Contains("RegisterPatientAsync", code);

            var cr = await wd.CompileAsync();
            cr.Errors.ForEach(Console.WriteLine);
            Assert.True(cr.Result, cr.ToString());

            var wfDll = Assembly.LoadFile(cr.Output);
            var wfType = wfDll.GetType($"{wd.CodeNamespace}.{wd.WorkflowTypeName}");
            dynamic wf = Activator.CreateInstance(wfType);


            Assert.NotNull(wf.patient);

            var patientDll = Assembly.LoadFrom(location);
            var patientType = patientDll.GetType($"Bespoke.{ConfigurationManager.ApplicationName}.Patients.Domain.Patient");
            Assert.NotNull(patientType);
            dynamic patient = Activator.CreateInstance(patientType);
            patient.Mrn = "784529";

            Assert.Equal("".GetType(), typeof(string));
            //Assert.Equal(patient.GetType(), wf.patient.GetType());
            await wf.RegisterPatientAsync(patient);
            Assert.Equal(patient.Mrn, wf.patient.Mrn);

        }

        [Fact]
        public async Task ReceveiveWithCorrelationSet()
        {
            var patientClassName = $"Bespoke.{ConfigurationManager.ApplicationName}.Patients.Domain.Patient";
            var patientDllName = $"{ConfigurationManager.ApplicationName}.Patient";
            var wd = new WorkflowDefinition { Name = "Receive Register new patient", Id = "receive-register-patient", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "mrn", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ClrTypeVariable { Name = "pesakit", TypeName = $"{patientClassName}, {patientDllName}", CanInitiateWithDefaultConstructor = true, WebId = Strings.GenerateId() });
            wd.ReferencedAssemblyCollection.Add(new ReferencedAssembly { Location = $"{ConfigurationManager.CompilerOutputPath}\\{ConfigurationManager.ApplicationName}.Patient.dll" });

            var crt = new CorrelationType { Name = "Mrn" };
            crt.CorrelationPropertyCollection.Add(new CorrelationProperty { Path = "pesakit.Mrn" });
            crt.CorrelationPropertyCollection.Add(new CorrelationProperty { Path = "pesakit.FullName" });
            wd.CorrelationTypeCollection.Add(crt);
            wd.CorrelationSetCollection.Add(new CorrelationSet { Type = crt.Name, Name = "mrn" });

            var start = new ExpressionActivity
            {
                IsInitiator = true,
                WebId = "start",
                Name = "Start Test Receive Workflow",
                NextActivityWebId = "register",
                Expression = "await this.InitializeCorrelationSetAsync(\"mrn\",\"784529;Adam\");"
            };
            wd.ActivityCollection.Add(start);

            var reg = new ReceiveActivity
            {
                Name = "Register",
                Operation = "Register",
                MessagePath = "pesakit",
                NextActivityWebId = "B",
                IsInitiator = false,
                WebId = "register"
            };
            reg.FollowingCorrelationSetCollection.Add("mrn");

            wd.ActivityCollection.Add(reg);


            var end = new EndActivity
            {
                WebId = "End",
                Name = "End"
            };
            wd.ActivityCollection.Add(end);

            var cr = await wd.CompileAsync();
            cr.Errors.ForEach(x => m_helper.WriteLine(x.ToString()));
            Assert.True(cr.Result, cr.ToString());

            var wfDll = Assembly.LoadFile(cr.Output);
            var wfType = wfDll.GetType($"{wd.CodeNamespace}.{wd.WorkflowTypeName}");
            dynamic wf = Activator.CreateInstance(wfType);


            Assert.NotNull(wf.pesakit);

            var patientDll = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == patientDllName);
            var patientType = patientDll.GetType(patientClassName);
            Assert.NotNull(patientType);
            dynamic patient = Activator.CreateInstance(patientType);
            patient.Mrn = "784529";
            patient.FullName = "Adam";

            Assert.Equal("".GetType(), typeof(string));
            Assert.Equal(patient.GetType(), wf.pesakit.GetType());
            await wf.RegisterAsync(patient);
            Assert.Equal(patient.Mrn, wf.pesakit.Mrn);

        }

        [Fact]
        public void ReceveiveWithoutCorrelationSet()
        {
            const string PATIENT_TYPE_FULL_NAME = "Bespoke.Dev_patient.Domain.Patient";
            var wd = new WorkflowDefinition { Name = "Receive Register new patient", Id = "receive-register-patient", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "mrn", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "patient", TypeName = PATIENT_TYPE_FULL_NAME });

            var register = new ReceiveActivity
            {
                Name = "Register",
                Operation = "Register",
                MessagePath = "patient",
                NextActivityWebId = "End",
                IsInitiator = false,
                WebId = "Register"
            };
            var end = new EndActivity { WebId = "End", Name = "End" };
            wd.ActivityCollection.Add(register);
            wd.ActivityCollection.Add(end);


            var br = wd.ValidateBuild();
            br.Errors.ForEach(e => Console.WriteLine(e.Message));
            Assert.Equal(false, br.Result);

        }

        [Fact]
        public async Task ReceveiveCorrelationSet()
        {
            // ReSharper disable once InconsistentNaming
            string PATIENT_TYPE_FULL_NAME = $"Bespoke.{ConfigurationManager.ApplicationName}.Patients.Domain.Patient, {ConfigurationManager.ApplicationName}.Patient";
            var wd = new WorkflowDefinition { Name = "Receive Register new patient", Id = "receive-register-patient", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "mrn", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "patient", TypeName = PATIENT_TYPE_FULL_NAME });

            var mrn = new CorrelationType { Name = "mrn" };
            mrn.CorrelationPropertyCollection.Add(new CorrelationProperty { Name = "mrn", Path = "patient.Mrn" });
            wd.CorrelationTypeCollection.Add(mrn);

            wd.ReferencedAssemblyCollection.Add(new ReferencedAssembly
            {
                Name = $"{ConfigurationManager.ApplicationName}.Patient",
                Location = $"{ConfigurationManager.CompilerOutputPath}\\{ConfigurationManager.ApplicationName}.Patient.dll"
            });

            var start = new ExpressionActivity
            {
                WebId = "Start",
                Name = "Start",
                Expression = "Console.WriteLine(DateTime.Now);",
                IsInitiator = true
            };
            wd.ActivityCollection.Add(start);

            var register = new ReceiveActivity
            {
                Name = "Register",
                Operation = "Register",
                MessagePath = "patient",
                NextActivityWebId = "End",
                IsInitiator = false,
                WebId = "Register"
            };
            register.FollowingCorrelationSetCollection.Add("mrn");


            var end = new EndActivity { WebId = "End", Name = "End" };
            wd.ActivityCollection.Add(register);
            wd.ActivityCollection.Add(end);

            var br = wd.ValidateBuild();
            br.Errors.ForEach(e => m_helper.WriteLine(e.Message));
            Assert.True(br.Result);


            var code = register.GenerateExecMethodBody(wd);
            Assert.Contains("RegisterPatientAsync", code);

            var cr = await wd.CompileAsync();
            cr.Errors.ForEach(Console.WriteLine);
            Assert.True(cr.Result);

            var wfDll = Assembly.LoadFile(cr.Output);
            var wfType = wfDll.GetType("Bespoke.Sph.Workflows_ReceiveRegisterPatient_0.ReceiveRegisterPatientWorkflow");
            dynamic wf = Activator.CreateInstance(wfType);


            Assert.NotNull(wf.patient);

            var patientDll = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "Dev.Patient");
            var patientType = patientDll.GetType(PATIENT_TYPE_FULL_NAME);
            Assert.NotNull(patientType);
            dynamic patient = Activator.CreateInstance(patientType);
            patient.Mrn = "784529";

            Assert.Equal("".GetType(), typeof(string));
            Assert.Equal(patient.GetType(), wf.patient.GetType(), "Type should be the same");
            await wf.RegisterPatientAsync(patient);
            Assert.Equal(patient.Mrn, wf.patient.Mrn);

        }

    }
}