﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;
using NUnit.Framework;

namespace domain.test.workflows
{
    [TestFixture]
    public class ReceiveActivityTestFixture
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
        public async Task ReceiveAndInitiate()
        {
            const string PATIENT_TYPE_FULL_NAME = "Bespoke.Dev_patient.Domain.Patient";
            var wd = new WorkflowDefinition { Name = "Receive Register new patient", Id = "receive-register-patient", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "mrn", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "patient", TypeName = PATIENT_TYPE_FULL_NAME });

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

            var code = regiterStaff.GenerateExecMethodBody(wd);
            StringAssert.Contains("RegisterPatientAsync", code);

            var options = new CompilerOptions();


            var cr = await wd.CompileAsync(options).ConfigureAwait(false);
            cr.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(cr.Result);

            var wfDll = Assembly.Load(cr.Buffer);
            var wfType = wfDll.GetType("Bespoke.Sph.Workflows_ReceiveRegisterPatient_0.ReceiveRegisterPatientWorkflow");
            dynamic wf = Activator.CreateInstance(wfType);


            Assert.IsNotNull(wf.patient, "Default property should not be null");

            var patientDll = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "Dev.Patient");
            var patientType = patientDll.GetType(PATIENT_TYPE_FULL_NAME);
            Assert.IsNotNull(patientType);
            dynamic patient = Activator.CreateInstance(patientType);
            patient.Mrn = "784529";

            Assert.AreEqual("".GetType(), typeof(string), "String type should be the same");
            Assert.AreEqual(patient.GetType(), wf.patient.GetType(), "Type should be the same");
            await wf.RegisterPatientAsync(patient);
            Assert.AreEqual(patient.Mrn, wf.patient.Mrn);

        }
        [Test]
        public async Task ReceveiveWithCorrelationSet()
        {
            const string PATIENT_TYPE_FULL_NAME = "Bespoke.Dev_patient.Domain.Patient";
            var wd = new WorkflowDefinition { Name = "Receive Register new patient", Id = "receive-register-patient", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "mrn", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "patient", TypeName = PATIENT_TYPE_FULL_NAME });


            var crt = new CorrelationType { Name = "Mrn" };
            crt.CorrelationPropertyCollection.Add(new CorrelationProperty { Path = "patient.Mrn" });
            crt.CorrelationPropertyCollection.Add(new CorrelationProperty { Path = "patient.FullName" });
            wd.CorrelationTypeCollection.Add(crt);
            wd.CorrelationSetCollection.Add(new CorrelationSet { Type = crt.Name, Name = "mrn" });

            var start = new ExpressionActivity
            {
                IsInitiator = true,
                WebId = "Start",
                Name = "Start",
                NextActivityWebId = "Register",
                Expression = "await this.InitializeCorrelationSetAsync(\"mrn\",\"784529;Adam\");"
            };
            wd.ActivityCollection.Add(start);
            var reg = new ReceiveActivity
            {
                Name = "Register",
                Operation = "Register",
                MessagePath = "patient",
                NextActivityWebId = "B",
                IsInitiator = false,
                WebId = "Register"
            };
            reg.FollowingCorrelationSetCollection.Add("mrn");

            wd.ActivityCollection.Add(reg);


            var end = new EndActivity
            {
                WebId = "End",
                Name = "End"
            };
            wd.ActivityCollection.Add(end);

            var code = reg.GenerateExecMethodBody(wd);
            StringAssert.Contains("RegisterAsync", code);

            var options = new CompilerOptions();


            var cr =await wd.CompileAsync(options).ConfigureAwait(false);
            cr.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(cr.Result);

            var wfDll = Assembly.Load(cr.Buffer);
            var wfType = wfDll.GetType("Bespoke.Sph.Workflows_ReceiveRegisterPatient_0.ReceiveRegisterPatientWorkflow");
            dynamic wf = Activator.CreateInstance(wfType);


            Assert.IsNotNull(wf.patient, "Default property should not be null");

            var patientDll = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "Dev.Patient");
            var patientType = patientDll.GetType(PATIENT_TYPE_FULL_NAME);
            Assert.IsNotNull(patientType);
            dynamic patient = Activator.CreateInstance(patientType);
            patient.Mrn = "784529";
            patient.FullName = "Adam";

            Assert.AreEqual("".GetType(), typeof(string), "String type should be the same");
            Assert.AreEqual(patient.GetType(), wf.patient.GetType(), "Type should be the same");
            await wf.RegisterAsync(patient);
            Assert.AreEqual(patient.Mrn, wf.patient.Mrn);

        }
        [Test]
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
            Assert.AreEqual(false, br.Result);

        }

        [Test]
        public async Task ReceveiveCorrelationSet()
        {
            const string PATIENT_TYPE_FULL_NAME = "Bespoke.Dev_patient.Domain.Patient";
            var wd = new WorkflowDefinition { Name = "Receive Register new patient", Id = "receive-register-patient", SchemaStoreId = m_schemaStoreId };
            wd.VariableDefinitionCollection.Add(new SimpleVariable { Name = "mrn", Type = typeof(string) });
            wd.VariableDefinitionCollection.Add(new ComplexVariable { Name = "patient", TypeName = PATIENT_TYPE_FULL_NAME });


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
            var end = new EndActivity { WebId = "End", Name = "End" };
            wd.ActivityCollection.Add(register);
            wd.ActivityCollection.Add(end);

            var br = wd.ValidateBuild();
            br.Errors.ForEach(e => Console.WriteLine(e.Message));
            Assert.IsTrue(br.Result);


            var code = register.GenerateExecMethodBody(wd);
            StringAssert.Contains("RegisterPatientAsync", code);

            var options = new CompilerOptions();


            var cr =await wd.CompileAsync(options).ConfigureAwait(false);
            cr.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(cr.Result);

            var wfDll = Assembly.Load(cr.Buffer);
            var wfType = wfDll.GetType("Bespoke.Sph.Workflows_ReceiveRegisterPatient_0.ReceiveRegisterPatientWorkflow");
            dynamic wf = Activator.CreateInstance(wfType);


            Assert.IsNotNull(wf.patient, "Default property should not be null");

            var patientDll = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "Dev.Patient");
            var patientType = patientDll.GetType(PATIENT_TYPE_FULL_NAME);
            Assert.IsNotNull(patientType);
            dynamic patient = Activator.CreateInstance(patientType);
            patient.Mrn = "784529";

            Assert.AreEqual("".GetType(), typeof(string), "String type should be the same");
            Assert.AreEqual(patient.GetType(), wf.patient.GetType(), "Type should be the same");
            await wf.RegisterPatientAsync(patient);
            Assert.AreEqual(patient.Mrn, wf.patient.Mrn);

        }


    }
}