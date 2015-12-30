using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
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
            var core = Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\core.sph.dll");
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(core);
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll"));

            var destinationCore = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "core.sph.dll");
            File.Copy(core, destinationCore, true);


            var codes = ed.GenerateCode();
            var sources = ed.SaveSources(codes);
            var result = ed.Compile(options, sources);

            result.Errors.ForEach(Console.WriteLine);

            // try to instantiate the EntityDefinition
            var assembly = Assembly.LoadFrom(result.Output);
            var edTypeName = $"Bespoke.{ConfigurationManager.ApplicationName}_{ed.Id}.Domain.{ed.Name}";

            var edType = assembly.GetType(edTypeName);
            Assert.IsNotNull(edType, edTypeName + " is null");

            return Activator.CreateInstance(edType);
        }


        public EntityDefinition CreatePatientDefinition(string name = "Patient")
        {
            var ent = new EntityDefinition { Name = name, Plural = "Patients", RecordName = "FullName", Id = "patient" };
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


            var contacts = new Member { Name = "ContactCollection", Type = typeof(Array) };
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
        public void PostRegister()
        {
            var release = new EntityOperation { Name = "Register", IsHttpPost = true, WebId = "abc" };
            release.Rules.Add("VerifyRegisteredDate");

            var ed = this.CreatePatientDefinition("PatientRegistered");
            ed.EntityOperationCollection.Add(release);

            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);

            Type patientType = patient.GetType();
            var dll = patientType.Assembly;
            foreach (var type in dll.GetTypes())
            {
                Console.WriteLine(type);
            }
            var controllerType = dll.GetType(patientType.Namespace + ".PatientRegisteredController");
            Assert.IsNotNull(controllerType);

            var releaseActionMethodInfo = controllerType.GetMethod("PostRegister");
            Assert.IsNotNull(releaseActionMethodInfo);

        }
        [Test]
        public void HttpPatchReleaseOperation()
        {
            var release = new EntityOperation { Name = "Release", IsHttpPatch = true, WebId = "ReleaseWithPatch" };
            release.PatchPathCollection.Add(new PatchSetter { Path = "Status" });
            //release.PatchPathCollection.Add("ClinicalNote");
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

            var releaseActionMethodInfo = controllerType.GetMethod("PatchRelease");
            Assert.IsNotNull(releaseActionMethodInfo);


        }
        [Test]
        public void HttpPutAdmit()
        {
            var admit = new EntityOperation { Name = "Admit", IsHttpPut = true, WebId = "PutAdmit" };
            admit.PatchPathCollection.Add(new PatchSetter { Path = "Status" });

            var ed = this.CreatePatientDefinition("PatientPutAdmit");
            ed.EntityOperationCollection.Add(admit);

            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);

            Type patientType = patient.GetType();
            var dll = patientType.Assembly;
            foreach (var type in dll.GetTypes())
            {
                Console.WriteLine(type);
            }
            var controllerType = dll.GetType(patientType.Namespace + ".PatientPutAdmitController");
            Assert.IsNotNull(controllerType);

            var releaseActionMethodInfo = controllerType.GetMethod("PutAdmit");
            Assert.IsNotNull(releaseActionMethodInfo);


        }


        [Test]
        public void HttpDelete()
        {
            var delete = new EntityOperation { Name = "Remove", Route = "", IsHttpDelete = true, WebId = "remove" };

            var ed = this.CreatePatientDefinition("PatientDelete");
            ed.EntityOperationCollection.Add(delete);

            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);

            Type patientType = patient.GetType();
            var dll = patientType.Assembly;
            foreach (var type in dll.GetTypes())
            {
                Console.WriteLine(type);
            }
            var controllerType = dll.GetType(patientType.Namespace + ".PatientDeleteController");
            Assert.IsNotNull(controllerType);

            var remove = controllerType.GetMethod("DeleteRemove");
            Assert.IsNotNull(remove);

            var httpDelete = remove.CustomAttributes.SingleOrDefault(x => x.AttributeType == typeof(HttpDeleteAttribute));
            Assert.IsNotNull(httpDelete);

            var parameters = remove.GetParameters();
            Assert.AreEqual(1, parameters.Length, "DELETE action should only contains 1 parameter{id}");
            var id = parameters.Single();
            Assert.AreEqual("id", id.Name);
            Assert.AreEqual(typeof(string), id.ParameterType);
        }
        [Test]
        public void HttpDeleteWithRule()
        {
            var delete = new EntityOperation { Name = "Remove", Route = "", IsHttpDelete = true, WebId = "remove" };
         
            const string NAME = "PatientDeleteWithRule";
            var ed = this.CreatePatientDefinition(NAME);
            ed.EntityOperationCollection.Add(delete);
            var rule = new BusinessRule {Name = "Cannot delete admitted patient", WebId = "rule01"};
            rule.RuleCollection.Add(new Rule { Left = new DocumentField {Path = "Status"} , Operator = Operator.Neq, Right = new ConstantField {Value = "Admitter", Type = typeof(string)} });
            ed.BusinessRuleCollection.Add(rule);
            delete.Rules.Add(rule.Name);

            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);

            Type patientType = patient.GetType();
            var dll = patientType.Assembly;
            foreach (var type in dll.GetTypes())
            {
                Console.WriteLine(type);
            }
            var controllerType = dll.GetType($"{patientType.Namespace}.{NAME}Controller");
            Assert.IsNotNull(controllerType);

            var remove = controllerType.GetMethod("DeleteRemove");
            Assert.IsNotNull(remove);

            var httpDelete = remove.CustomAttributes.SingleOrDefault(x => x.AttributeType == typeof(HttpDeleteAttribute));
            Assert.IsNotNull(httpDelete);

            var parameters = remove.GetParameters();
            Assert.AreEqual(1, parameters.Length, "DELETE action should only contains 1 parameter{id}");
            var id = parameters.Single();
            Assert.AreEqual("id", id.Name);
            Assert.AreEqual(typeof(string), id.ParameterType);
        }


        private dynamic AddMockRespository(Type edType)
        {
            var mock = typeof(MockRepository<>);
            var reposType = mock.MakeGenericType(edType);
            var repository = Activator.CreateInstance(reposType);

            var ff = typeof(IRepository<>).MakeGenericType(edType);

            ObjectBuilder.AddCacheList(ff, repository);

            return repository;
        }

        [Test]
        public async Task PatchReleaseOperationWithBusinessRule()
        {
            var release = new EntityOperation { Name = "Release", IsHttpPatch = true, WebId = "1" };
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
            string jsonPath = $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\{ed.Id}.json";
            File.WriteAllText(jsonPath, ed.ToJsonString(true));

            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);
            patient.Id = Guid.NewGuid().ToString();
            patient.DeathDateTime = DateTime.Today.AddDays(1);

            Type patientType = patient.GetType();
            var dll = patientType.Assembly;

            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var controllerType = dll.GetType(patientType.Namespace + ".PatientWithBusinessRuleController");
            dynamic controller = Activator.CreateInstance(controllerType);

            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ed.Clone());


            var result = await controller.PatchRelease(patient.Id, JsonSerializerService.ToJsonString(patient, true));
            Console.WriteLine("Result type : " + result);
            Assert.IsNotNull(result);

            dynamic vr = result.Data;
            var ttt = JsonSerializerService.ToJsonString(vr, Formatting.Indented);
            StringAssert.Contains("\"success\": false", ttt);
            Console.WriteLine();
            //Assert.IsFalse(vr.success);
            //Assert.AreEqual(3, vr.rules.Length);
            if (File.Exists(jsonPath))
                File.Delete(jsonPath);

        }
        [Test]
        public async Task PatchReleaseOperationWithSetter()
        {
            var release = new EntityOperation { Name = "Release", IsHttpPatch = true, WebId = Guid.NewGuid().ToString() };
            var statusSetter = new SetterActionChild
            {
                Path = "Status",
                Field = new ConstantField { Type = typeof(string), Value = "Released", Name = "Released" },
                WebId = Guid.NewGuid().ToString()
            };
            var ed = this.CreatePatientDefinition("PatientReleaseOperationWithSetter");
            ed.EntityOperationCollection.Add(release);
            release.SetterActionChildCollection.Add(statusSetter);
            string jsonPath = $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\{ed.Id}.json";
            File.WriteAllText(jsonPath, ed.ToJsonString(true));


            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);
            patient.DeathDateTime = DateTime.Today.AddDays(1);
            patient.Id = Guid.NewGuid().ToString();


            Type patientType = patient.GetType();
            var dll = patientType.Assembly;
            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var controllerType = dll.GetType(patientType.Namespace + ".PatientReleaseOperationWithSetterController");
            dynamic controller = Activator.CreateInstance(controllerType);

            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ed.Clone());


            var result = await controller.PatchRelease(patient.Id, JsonSerializerService.ToJsonString(patient, true));
            Console.WriteLine("Result type : " + result);
            Assert.IsNotNull(result);

            dynamic vr = result.Data;
            Assert.IsNotNull(vr);

            Assert.AreEqual("Released", patient.Status);

        }
    }
}
