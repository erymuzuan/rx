using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using domain.test.reports;
using domain.test.triggers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace domain.test.entities
{
    [TestFixture]
    public class OperationTest
    {
        private MockRepository<EntityDefinition> m_efMock;
        private readonly MockPersistence m_persistence = new MockPersistence();

        [SetUp]
        public void Setup()
        {
            m_efMock = new MockRepository<EntityDefinition>();
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(m_efMock);
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<IPersistence>(m_persistence);
        }
        private dynamic CreateInstance(EntityDefinition ed, bool verbose = false)
        {
            var options = new CompilerOptions
            {
                IsVerbose = verbose,
                IsDebug = true
            };

            byte[] buffers;
            using (var stream = new MemoryStream())
            {
                options.Stream = stream;
                options.Emit = true;
                var result = ed.Compile(options);
                EntityDefinitionCodeTest.PrintErrors(result, ed);

                Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));
                buffers = stream.GetBuffer();
            }
            
            // try to instantiate the EntityDefinition
            var assembly = Assembly.Load(buffers);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed.Id, ed.Name);

            var edType = assembly.GetType(edTypeName);
            Assert.IsNotNull(edType, edTypeName + " is null");

            return Activator.CreateInstance(edType);
        }


        public EntityDefinition CreatePatientDefinition(string name = "Patient")
        {
            var ent = new EntityDefinition { Name = name, Plural = "Patients", RecordName = "FullName" };
            ent.MemberCollection.Add(new Member
            {
                Name = "FullName",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            ent.MemberCollection.Add(new Member
            {
                Name = "Status",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            ent.MemberCollection.Add(new Member
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            ent.MemberCollection.Add(new Member
            {
                Name = "DeathDateTime",
                TypeName = "System.DateTime, mscorlib",
                IsFilterable = true
            });
            var address = new Member { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new Member { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new Member { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);


            var contacts = new Member { Name = "ContactCollection", AllowMultiple = true };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ent.MemberCollection.Add(contacts);

            return ent;

        }

        [Test]
        public void GeneratePatientTest()
        {
            var ed = this.CreatePatientDefinition();
            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);
        }

        [Test]
        public void AddReleaseOperation()
        {
            var release = new EntityOperation { Name = "Release" };
            release.Rules.Add("VerifyRegisteredDate");

            var ed = this.CreatePatientDefinition("PatientForRelease");
            ed.EntityOperationCollection.Add(release);

            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);

            Type patientType = patient.GetType();
            var dll = patientType.Assembly;
            foreach (var type in dll.GetTypes())
            {
                Console.WriteLine(type);
            }
            var controllerType = dll.GetType(patientType.Namespace + ".PatientForReleaseController");
            Assert.IsNotNull(controllerType);

            var releaseActionMethodInfo = controllerType.GetMethod("Release");
            Assert.IsNotNull(releaseActionMethodInfo);

        }
        [Test]
        public async Task AddReleaseOperationWithBusinessRule()
        {
            var release = new EntityOperation { Name = "Release" };
            release.Rules.Add("Must be dead");
            release.Rules.Add("Must be registered");

            var mustBeDeadRule = new BusinessRule { Name = "Must be dead" };
            mustBeDeadRule.RuleCollection.Add(new Rule
            {
                Left = new DocumentField { Path = "DeathDateTime" },
                Operator = Operator.Lt,
                Right = new FunctionField { Script = "DateTime.Today" }
            });
            var mustBeRegisteredRule = new BusinessRule { Name = "Must be registered" };
            mustBeRegisteredRule.RuleCollection.Add(new Rule
            {
                Left = new DocumentField { Path = "RegisteredDateTime" },
                Operator = Operator.Gt,
                Right = new DocumentField { Path = "DeathDateTime" }
            });

            var ed = this.CreatePatientDefinition("PatientWithBusinessRule");
            ed.EntityOperationCollection.Add(release);
            ed.BusinessRuleCollection.Add(mustBeDeadRule);

            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);
            patient.DeathDateTime = DateTime.Today.AddDays(1);

            Type patientType = patient.GetType();
            var dll = patientType.Assembly;

            var controllerType = dll.GetType(patientType.Namespace + ".PatientWithBusinessRuleController");
            dynamic controller = Activator.CreateInstance(controllerType);

            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ed.Clone());


            var result = await controller.Release(patient);
            Console.WriteLine("Result type : " + result);
            Assert.IsNotNull(result);

            dynamic vr = result.Data;
            var ttt = JsonSerializerService.ToJsonString(vr, Formatting.Indented);
            StringAssert.Contains("\"success\": false", ttt);
            Console.WriteLine();
            //Assert.IsFalse(vr.success);
            //Assert.AreEqual(3, vr.rules.Length);

        }
        [Test]
        public async Task AddReleaseOperationWithSetter()
        {
            var release = new EntityOperation { Name = "Release", WebId = Guid.NewGuid().ToString() };
            var statusSetter = new SetterActionChild
            {
                Path = "Status",
                Field = new ConstantField { Type = typeof(string), Value = "Released", Name = "Released" },
                WebId = Guid.NewGuid().ToString()
            };
            var ed = this.CreatePatientDefinition("PatientReleaseOperationWithSetter");
            ed.EntityOperationCollection.Add(release);
            release.SetterActionChildCollection.Add(statusSetter);

            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);
            patient.DeathDateTime = DateTime.Today.AddDays(1);

            Type patientType = patient.GetType();
            var dll = patientType.Assembly;

            var controllerType = dll.GetType(patientType.Namespace + ".PatientReleaseOperationWithSetterController");
            dynamic controller = Activator.CreateInstance(controllerType);

            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ed.Clone());


            var result = await controller.Release(patient);
            Console.WriteLine("Result type : " + result);
            Assert.IsNotNull(result);

            dynamic vr = result.Data;
            Assert.IsNotNull(vr);

            Assert.AreEqual("Released", patient.Status);
            //Assert.IsFalse(vr.success);
            //Assert.AreEqual(3, vr.rules.Length);

        }
    }
}
