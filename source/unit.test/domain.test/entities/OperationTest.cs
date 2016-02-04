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
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

            var cache = new Mock<ICacheManager>();
            cache.Setup(x => x.Get<QueryEndpoint>(It.IsAny<string>()))
                .Returns(new QueryEndpoint());
            ObjectBuilder.AddCacheList(cache.Object);
        }
        private dynamic CreateInstance(EntityDefinition ed, bool verbose = false)
        {
            var options = new CompilerOptions
            {
                IsVerbose = verbose,
                IsDebug = true
            };
            var core = Path.GetFullPath($@"{ConfigurationManager.WebPath}\bin\core.sph.dll");
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath($@"{ConfigurationManager.WebPath}\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(core);
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath($@"{ConfigurationManager.WebPath}\bin\Newtonsoft.Json.dll"));

            var destinationCore = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "core.sph.dll");
            File.Copy(core, destinationCore, true);


            var codes = ed.GenerateCode();
            var sources = ed.SaveSources(codes);
            var result = ed.Compile(options, sources);

            result.Errors.ForEach(Console.WriteLine);

            // try to instantiate the EntityDefinition
            var assembly = Assembly.LoadFrom(result.Output);
            var edTypeName = $"{ed.CodeNamespace}.{ed.Name}";

            var edType = assembly.GetType(edTypeName);
            Assert.IsNotNull(edType, edTypeName + " is null");

            return Activator.CreateInstance(edType);
        }


        public EntityDefinition CreatePatientDefinition(string name = "TestingPatient")
        {
            var ent = new EntityDefinition { Name = name, Plural = "Patients", RecordName = "FullName", Id = "patient" };
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "FullName",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Status",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "DeathDateTime",
                TypeName = "System.DateTime, mscorlib",
                IsFilterable = true,
                IsNullable = true
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Discharged",
                TypeName = "System.DateTime, mscorlib",
                IsFilterable = true
            });
            var address = new ComplexMember { Name = "Address", TypeName = "Address" };
            address.MemberCollection.Add(new SimpleMember { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);


            var contacts = new ComplexMember { Name = "ContactCollection", TypeName = "Contact" };
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
        public async Task PostRegister()
        {
            var release = new OperationEndpoint
            {
                Name = "Register",
                Resource = "patients",
                Entity = "PatientRegistered",
                IsHttpPost = true,
                WebId = "abc",
                Id = "patient-register"
            };
            release.AddRules("VerifyRegisteredDate");

            var ed = this.CreatePatientDefinition(release.Entity);
            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);

            var result = await release.CompileAsync(ed);
            Assert.IsTrue(result.Result, result.ToString());

            var dll = Assembly.LoadFrom(result.Output);

            var controllerType = dll.GetType($"{release.CodeNamespace}.{release.Name}Controller");
            dynamic controller = Activator.CreateInstance(controllerType);

            var response = await controller.PostRegister(patient);
            JObject json = JObject.Parse(JsonSerializerService.ToJsonString(response.Data));

            var id = json.SelectToken("$.id").Value<string>();
            Assert.AreEqual(json.SelectToken("$._links.href").Value<string>(), $"{ConfigurationManager.BaseUrl}/api/patients/" + id);

        }
        [Test]
        public void HttpPatchReleaseOperation()
        {
            var release = new OperationEndpoint { Name = "Release", IsHttpPatch = true, WebId = "ReleaseWithPatch" };
            release.PatchPathCollection.Add(new PatchSetter { Path = "Status" });
            //release.PatchPathCollection.Add("ClinicalNote");
            release.Rules.Add("VerifyRegisteredDate");


            var ed = this.CreatePatientDefinition("PatientForRelease");


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
            var admit = new OperationEndpoint { Name = "Admit", IsHttpPut = true, WebId = "PutAdmit" };
            admit.PatchPathCollection.Add(new PatchSetter { Path = "Status" });

            var ed = this.CreatePatientDefinition("PatientPutAdmit");

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
            var delete = new OperationEndpoint { Name = "Remove", Route = "", IsHttpDelete = true, WebId = "remove" };

            var ed = this.CreatePatientDefinition("PatientDelete");

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
            var delete = new OperationEndpoint { Name = "Remove", Route = "", IsHttpDelete = true, WebId = "remove" };

            const string NAME = "PatientDeleteWithRule";
            var ed = this.CreatePatientDefinition(NAME);
            var rule = new BusinessRule { Name = "Cannot delete admitted patient", WebId = "rule01" };
            rule.RuleCollection.Add(new Rule { Left = new DocumentField { Path = "Status" }, Operator = Operator.Neq, Right = new ConstantField { Value = "Admitter", Type = typeof(string) } });
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
        public async Task ConflictDetection()
        {
            var mortuary = new OperationEndpoint
            {
                Name = "SendToMortuary",
                Resource = "patients",
                Entity = "PatientWithConflictDetection",
                Route = "{id:guid}/release-with-conflict-detection",
                IsHttpPatch = true,
                WebId = "1",
                IsConflictDetectionEnabled = true
            };

            mortuary.PatchPathCollection.Add(new PatchSetter { Path = "DeathDateTime", IsRequired = true });

            var ed = this.CreatePatientDefinition(mortuary.Entity);
            string jsonPath = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(EntityDefinition)}\\{ed.Id}.json";
            string oePath = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(OperationEndpoint)}\\{mortuary.Id}.json";
            File.WriteAllText(jsonPath, ed.ToJsonString(true));
            File.WriteAllText(oePath, mortuary.ToJsonString(true));

            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);
            patient.Id = "0142ae18-a205-4979-9218-39f92b41589e";
            patient.DeathDateTime = DateTime.Today.AddDays(1);


            var cr = await mortuary.CompileAsync(ed);
            Assert.IsTrue(cr.Result, cr.ToString());

            Type patientType = patient.GetType();
            var oedll = Assembly.LoadFrom(cr.Output);

            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var controllerType = oedll.GetType($"{mortuary.CodeNamespace}.{mortuary.Name}Controller");
            dynamic controller = Activator.CreateInstance(controllerType);
            var context = ((Controller)controller).SetContext();

            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ed.Clone());

            context.Request.Headers.Add("If-None-Matched", "45");

            var result = await controller.PatchSendToMortuary(patient.Id, JsonSerializerService.ToJsonString(patient, true));
            Console.WriteLine("Result type : " + result);
            Assert.IsNotNull(result);
            Assert.AreEqual(409, context.Response.StatusCode);


            dynamic vr = result.Data;
            var ttt = JsonSerializerService.ToJsonString(vr, Formatting.Indented);
            StringAssert.Contains("\"success\": false", ttt);
            Console.WriteLine();
            //Assert.IsFalse(vr.success);
            //Assert.AreEqual(3, vr.rules.Length);
            File.Delete(jsonPath);
            File.Delete(oePath);

        }

        [Test]
        public async Task PatchReleaseOperationWithBusinessRule()
        {
            var release = new OperationEndpoint
            {
                Name = "Release",
                Resource = "patients",
                Entity = "PatientWithBusinessRule",
                Route = "{id:guid}/release-with-business-rule",
                IsHttpPatch = true,
                WebId = "1"
            };
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

            var ed = this.CreatePatientDefinition(release.Entity);
            ed.BusinessRuleCollection.Add(mustBeDeadRule);
            string jsonPath = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(EntityDefinition)}\\{ed.Id}.json";
            string oePath = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(OperationEndpoint)}\\{release.Id}.json";
            File.WriteAllText(jsonPath, ed.ToJsonString(true));
            File.WriteAllText(oePath, release.ToJsonString(true));

            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);
            patient.Id = Guid.NewGuid().ToString();
            patient.DeathDateTime = DateTime.Today.AddDays(1);


            var cr = await release.CompileAsync(ed);

            Type patientType = patient.GetType();
            var oedll = Assembly.LoadFrom(cr.Output);

            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var controllerType = oedll.GetType($"{release.CodeNamespace}.{release.Name}Controller");
            dynamic controller = Activator.CreateInstance(controllerType);
            MvcControllerHelper.SetContext(controller);

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
            File.Delete(jsonPath);
            File.Delete(oePath);

        }
        [Test]
        public async Task PatchReleaseOperationWithSetter()
        {
            var release = new OperationEndpoint
            {
                Name = "Release",
                Id = "test-relase-endpoint-with-setter",
                Resource = "patients",
                Entity = "PatientReleaseOperationWithSetter",
                IsHttpPatch = true,
                Route = "{id}/release"
            };
            release.PatchPathCollection.Add(new PatchSetter { Path = "FullName", IsRequired = true });

            var statusSetter = new SetterActionChild
            {
                Path = "Status",
                Field = new ConstantField { Type = typeof(string), Value = "Released", Name = "Released" },
                WebId = Guid.NewGuid().ToString()
            };
            release.SetterActionChildCollection.Add(statusSetter);

            var dobSetter = new SetterActionChild
            {
                Path = "Discharged",
                Field = new FunctionField { Script = "item.CreatedDate == DateTime.MinValue ? DateTime.Now : item.CreatedDate", Name = "Dob" },
                WebId = Guid.NewGuid().ToString()
            };
            release.SetterActionChildCollection.Add(dobSetter);

            var ed = this.CreatePatientDefinition(release.Entity);
            string edPath = $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\{ed.Id}.json";
            File.WriteAllText(edPath, ed.ToJsonString(true));


            var oePath = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(OperationEndpoint)}\\{release.Id}.json";
            File.WriteAllText(oePath, release.ToJsonString(true));


            var patient = this.CreateInstance(ed, true);
            Assert.IsNotNull(patient);
            patient.DeathDateTime = DateTime.Today.AddDays(1);
            patient.Id = Guid.NewGuid().ToString();


            Type patientType = patient.GetType();
            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var cr = await release.CompileAsync(ed);
            Assert.IsTrue(cr.Result, cr.ToString());
            var opDll = Assembly.LoadFrom(cr.Output);

            var controller = opDll.CreateController(release.CodeNamespace + ".ReleaseController");
            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ed.Clone());

            var result = await controller.PatchRelease(patient.Id, JsonSerializerService.ToJsonString(patient, true));
            Console.WriteLine("Result type : " + result);
            Assert.IsNotNull(result);

            dynamic vr = result.Data;
            Assert.IsNotNull(vr);

            Assert.AreEqual("Released", patient.Status);

            File.Delete(oePath);
            File.Delete(edPath);

        }
    }
}
