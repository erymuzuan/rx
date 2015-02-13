using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Hosting;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using domain.test.reports;
using domain.test.triggers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine2());
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<IPersistence>(m_persistence);
        }

        private async Task<dynamic> CreateInstanceAsync(EntityDefinition ed, bool verbose = false)
        {
            var options = new CompilerOptions
            {
                IsVerbose = verbose,
                IsDebug = true
            };
            var guid = Guid.NewGuid();
            var folder = string.Format(@"{0}\{1}", Path.GetTempPath(), guid);
            Directory.CreateDirectory(folder);

            var temp = string.Format(@"{0}\{1}\{2}.{3}.dll", Path.GetTempPath(), guid, ConfigurationManager.ApplicationName, ed.Id);
            using (var stream = new FileStream(temp, FileMode.Create))
            {
                options.Stream = stream;
                options.Emit = true;
                var result = await ed.CompileAsync(options).ConfigureAwait(false);
                await EntityDefinitionCodeTest.PrintErrorsAsync(result, ed);
                Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));

            }

            var assembly = Assembly.LoadFile(temp);
            var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed.Id, ed.Name);

            var edType = assembly.GetType(edTypeName);
            Assert.IsNotNull(edType, edTypeName + " is null");


            AppDomain.CurrentDomain.AssemblyResolve += (o, e) =>
            {
                Console.WriteLine("Resolving {0}", e.Name);
                if (e.Name.StartsWith(ConfigurationManager.ApplicationName)) return assembly;
                var name = ConfigurationManager.WebPath + @"\bin\" + e.Name.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).First().Trim()
                    + ".dll";
                if (File.Exists(name))
                    return Assembly.LoadFile(name);
                Console.WriteLine("Cannot find " + name);
                return null;
            };

            return Activator.CreateInstance(edType);
        }


        public EntityDefinition CreatePatientDefinition(string name = "Patient")
        {
            var ent = new EntityDefinition
            {
                Name = name,
                Id = name.ToIdFormat(),
                Plural = "Patients",
                RecordName = "FullName"
            };
            ent.MemberCollection.Add(new Member
            {
                Name = "FullName",
                Type = typeof(string),
                IsFilterable = true
            });
            ent.MemberCollection.Add(new Member
            {
                Name = "Status",
                Type = typeof(string),
                IsFilterable = true
            });
            ent.MemberCollection.Add(new Member
            {
                Name = "Title",
                Type = typeof(string),
                IsFilterable = true
            });
            ent.MemberCollection.Add(new Member
            {
                Name = "DeathDateTime",
                Type = typeof(DateTime),
                IsNullable = true,
                IsFilterable = true
            });
            ent.MemberCollection.Add(new Member
            {
                Name = "RegisteredDateTime",
                Type = typeof(DateTime),
                IsFilterable = true
            });
            var address = new Member { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new Member { Name = "Street1", IsFilterable = false, Type = typeof(string)});
            address.MemberCollection.Add(new Member { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);


            var contacts = new Member { Name = "ContactCollection", AllowMultiple = true };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ent.MemberCollection.Add(contacts);

            return ent;

        }

        [Test]
        public async Task GeneratePatientTest()
        {
            var ed = this.CreatePatientDefinition();
            var patient = await this.CreateInstanceAsync(ed, true);
            Assert.IsNotNull(patient);
        }

        [Test]
        public async Task AddReleaseOperation()
        {
            var release = new EntityOperation { Name = "Release" };
            release.Rules.Add("VerifyRegisteredDate");

            var ed = this.CreatePatientDefinition("PatientForRelease");
            ed.EntityOperationCollection.Add(release);

            var patient = await this.CreateInstanceAsync(ed, true).ConfigureAwait(false);
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
            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ed.Clone());

            var patient = await this.CreateInstanceAsync(ed, true).ConfigureAwait(false);
            patient.RegisteredDateTime = DateTime.Today;
            patient.DeathDateTime = DateTime.Today.AddDays(1);

            Type patientType = patient.GetType();
            var dll = patientType.Assembly;

            var controllerType = dll.GetType(patientType.Namespace + ".PatientWithBusinessRuleController");
            dynamic controller = Activator.CreateInstance(controllerType);
            controller.Request = new HttpRequestMessage { Method = HttpMethod.Post };
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var result = await controller.Save(patient);

            string json = JsonConvert.SerializeObject(result.Content.Value, Formatting.Indented);
            var converter = new ExpandoObjectConverter();
            dynamic vr = JsonConvert.DeserializeObject<ExpandoObject>(json, converter);
            Assert.IsFalse(vr.success);
            Assert.AreEqual(3, vr.rules.Length);

        }


        [Test]
        public async Task AddReleaseOperationWithSetter()
        {
            var release = new EntityOperation { Name = "Release", WebId = Guid.NewGuid().ToString() };
            var setter = new SetterActionChild
            {
                Path = "Status",
                Field = new ConstantField { Type = typeof(string), Value = "Released", Name = "Released" },
                WebId = Guid.NewGuid().ToString()
            };
            var ed = this.CreatePatientDefinition("PatientReleaseOperationWithSetter");
            ed.EntityOperationCollection.Add(release);
            release.SetterActionChildCollection.Add(setter);

            var patient = await this.CreateInstanceAsync(ed, true).ConfigureAwait(false);
            Assert.IsNotNull(patient);
            patient.DeathDateTime = DateTime.Today.AddDays(1);

            Type patientType = patient.GetType();
            var dll = patientType.Assembly;

            var controllerType = dll.GetType(patientType.Namespace + ".PatientReleaseOperationWithSetterController");
            dynamic controller = Activator.CreateInstance(controllerType);

            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ed.Clone());

            controller.Request = new HttpRequestMessage();
            controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            var result = await controller.Release(patient);
            Console.WriteLine("Result type : " + result);
            Assert.IsNotNull(result);

            dynamic vr = DynamicExtensions.ToDynamic(result.Content.Value);
            Assert.IsNotNull(vr);

            Assert.AreEqual("Released", patient.Status);
            Assert.IsTrue(vr.success);

        }
    }
}
