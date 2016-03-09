using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using Bespoke.Sph.WebApi;
using domain.test.reports;
using domain.test.triggers;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.entities
{
    [Trait("Category", "Operation endpoints")]
    [Trait("Category", "Compile")]
    public class OperationTest
    {
        private readonly ITestOutputHelper m_output;
        private readonly MockRepository<EntityDefinition> m_efMock;
        private readonly MockPersistence m_persistence = new MockPersistence();
        private readonly Mock<ICacheManager> m_cache;

        public OperationTest(ITestOutputHelper output)
        {
            m_output = output;
            m_efMock = new MockRepository<EntityDefinition>();
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(m_efMock);
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<IPersistence>(m_persistence);

            m_cache = new Mock<ICacheManager>();
            m_cache.Setup(x => x.Get<QueryEndpoint>(It.IsAny<string>()))
                .Returns(new QueryEndpoint());
            ObjectBuilder.AddCacheList(m_cache.Object);
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
            Assert.NotNull(edType);

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

        [Fact]
        public void GeneratePatientTest()
        {
            var ed = this.CreatePatientDefinition();
            var patient = this.CreateInstance(ed, true);
            Assert.NotNull(patient);
        }

        [Theory]
        [Trait("Verb", "POST")]
        [InlineData("Register1", "Anonymous")]
        [InlineData("Register2", "Everybody")]
        [InlineData("Register3", "administrators")]
        [InlineData("Register4", "nurse,medical-assitant")]
        public async Task PostRegister(string name, string roles)
        {
            var endpoint = new OperationEndpoint
            {
                Name = name,
                Resource = "patients",
                Entity = "Patient" + name,
                IsHttpPost = true,
                WebId = "abc",
                Id = "patient-register",
                Performer = new Performer
                {
                    UserProperty = "Roles",
                    Value = roles
                }
            };
            endpoint.AddRules("VerifyRegisteredDate");

            var ed = this.CreatePatientDefinition(endpoint.Entity);
            var patient = this.CreateInstance(ed, true);
            Assert.NotNull(patient);

            m_cache.Setup(x => x.Get<EntityDefinition>(ed.Id))
                .Returns(ed);

            var result = await endpoint.CompileAsync(ed);
            Assert.True(result.Result, result.ToString());

            try
            {
                var dll = Assembly.LoadFrom(result.Output);

                var controllerType = dll.GetType($"{endpoint.CodeNamespace}.{endpoint.Name}Controller");
                var action = controllerType.GetMethod("Post" + name);
                var controller = Activator.CreateInstance(controllerType);
                var bs = controller as BaseApiController;
                Assert.NotNull(bs);
                bs.Configuration = new HttpConfiguration();

                var awaiter = (Task<IHttpActionResult>)action.Invoke(controller, new object[] { ed, endpoint, patient });
                var response = (AcceptedResult)await awaiter;
                var json = JObject.Parse(JsonConvert.SerializeObject(response.Result));

                var id = json.SelectToken("$.id").Value<string>();
                var location = json.SelectToken("$._links.href").Value<string>();
                Assert.Equal(location, $"{ConfigurationManager.BaseUrl}/api/patients/" + id);


                // check header location
                Assert.Equal(location, response.LocationUri.ToString());

                // 202
                Assert.Contains("Accepted", response.GetType().FullName);
            }
            catch (ReflectionTypeLoadException e)
            {
                m_output.WriteLine(e.ToString());
                foreach (var inner in e.LoaderExceptions)
                {
                    m_output.WriteLine(inner.ToString());
                }

                throw;
            }
        }
        [Fact]
        [Trait("Verb", "PATCH")]
        public async Task HttpPatchReleaseOperation()
        {
            var ed = this.CreatePatientDefinition("PatientForRelease");
            var patient = this.CreateInstance(ed, true);
            Assert.NotNull(patient);


            var release = new OperationEndpoint { Name = "Release", Entity = ed.Name, Resource = "patients", IsHttpPatch = true, WebId = "ReleaseWithPatch" };
            release.PatchPathCollection.Add(new PatchSetter { Path = "Status", DefaultValue = "\"Released\"" });
            //release.PatchPathCollection.Add("ClinicalNote");
            release.Rules.Add("VerifyRegisteredDate");

            var cr = await release.CompileAsync(ed);
            Assert.True(cr.Result, cr.ToString());


            var dll = Assembly.LoadFrom(cr.Output);
            var controllerType = dll.GetType($"{release.CodeNamespace}.{release.Name}Controller");
            Assert.NotNull(controllerType);

            var releaseActionMethodInfo = controllerType.GetMethod("PatchRelease");
            Assert.NotNull(releaseActionMethodInfo);


        }
        [Fact]
        [Trait("Verb", "PUT")]
        public async Task HttpPutAdmit()
        {
            var ed = this.CreatePatientDefinition("PatientPutAdmit");
            var admit = new OperationEndpoint { Name = "Admit", Id = "patient-admit", Entity = ed.Name, Resource = "patients", IsHttpPut = true, WebId = "PutAdmit" };
            admit.PatchPathCollection.Add(new PatchSetter { Path = "Status", DefaultValue = "\"Admitted\"" });


            var patient = this.CreateInstance(ed, true);
            Assert.NotNull(patient);

            var cr = await admit.CompileAsync(ed);
            Assert.True(cr.Result, cr.ToString());

            var dll = Assembly.LoadFrom(cr.Output);
            var controllerType = dll.GetType($"{admit.CodeNamespace}.{admit.Name}Controller");
            Assert.NotNull(controllerType);

            var releaseActionMethodInfo = controllerType.GetMethod($"Put{admit.Name}");
            Assert.NotNull(releaseActionMethodInfo);

        }

        [Fact]
        [Trait("Verb", "PUT")]
        [Trait("Category", "BUGS")]
        [Trait("BUG", "#3357")]
        public async Task HttpPutAdmitWithFieldSetter()
        {
            var ed = this.CreatePatientDefinition("PatientPutAdmitWithSetter");
            var admit = new OperationEndpoint { Name = "AdmitWithSetter", Entity = ed.Name, Resource = "patients", IsHttpPut = true, WebId = "PutAdmit" };
            admit.PatchPathCollection.Add(new PatchSetter { Path = "Status", DefaultValue = "\"Admitted\"" });

            admit.SetterActionChildCollection.Add(new SetterActionChild
            {
                Path = "Status",
                WebId = Strings.GenerateId(),
                Field = new ConstantField { Type = typeof(string), Value = "Admitted", Name = "Admitted" }
            });
            admit.SetterActionChildCollection.Add(new SetterActionChild
            {
                Path = "CreatedDate",
                WebId = Strings.GenerateId(),
                Field = new FunctionField { Script = "DateTime.Today", Name = "Today" }
            });
            admit.SetterActionChildCollection.Add(new SetterActionChild
            {
                Path = "ChangedDate",
                Field = new FunctionField { Script = "return DateTime.Now;", Name = "Now" }
            });
            admit.SetterActionChildCollection.Add(new SetterActionChild
            {
                Path = "CreatedBy",
                Field = new FunctionField
                {
                    Script = @"
                await Task.Delay(500);
                return item.FullName;",
                    Name = "Current user"
                }
            });

            var patient = this.CreateInstance(ed, true);
            Assert.NotNull(patient);

            var cr = await admit.CompileAsync(ed);
            Assert.True(cr.Result, cr.ToString());

            var dll = Assembly.LoadFrom(cr.Output);
            var controllerType = dll.GetType($"{admit.CodeNamespace}.{admit.Name}Controller");
            Assert.NotNull(controllerType);

            var releaseActionMethodInfo = controllerType.GetMethod($"Put{admit.Name}");
            Assert.NotNull(releaseActionMethodInfo);

        }


        [Fact]
        [Trait("Verb", "DELETE")]
        public async Task HttpDelete()
        {
            var ed = this.CreatePatientDefinition("PatientDelete");
            var patient = this.CreateInstance(ed);
            Assert.NotNull(patient);

            var delete = new OperationEndpoint { Name = "Remove", Entity = ed.Name, Route = "", IsHttpDelete = true, WebId = "remove" };


            var cr = await delete.CompileAsync(ed);
            Assert.True(cr.Result, cr.ToString());


            var dll = Assembly.LoadFrom(cr.Output);
            var controllerType = dll.GetType($"{delete.CodeNamespace}.{delete.Name}Controller");
            Assert.NotNull(controllerType);

            var remove = controllerType.GetMethod($"Delete{delete.Name}");
            Assert.NotNull(remove);

            var httpDelete = remove.CustomAttributes.SingleOrDefault(x => x.AttributeType == typeof(HttpDeleteAttribute));
            Assert.NotNull(httpDelete);

            var route = remove.CustomAttributes.SingleOrDefault(x => x.AttributeType == typeof(DeleteRouteAttribute));
            Assert.NotNull(route);

            var parameters = remove.GetParameters();
            Assert.Equal(3, parameters.Length);
            var id = parameters[2];
            Assert.Equal("id", id.Name);
            Assert.Equal(typeof(string), id.ParameterType);
        }
        [Fact]
        [Trait("Verb", "DELETE")]
        public async Task HttpDeleteWithRule()
        {
            const string NAME = "PatientDeleteWithRule";
            var delete = new OperationEndpoint { Name = "Remove", Id = "patient-remove", Entity = NAME, Resource = "patients", Route = "", IsHttpDelete = true, WebId = "remove" };

            var ed = this.CreatePatientDefinition(NAME);
            var rule = new BusinessRule { Name = "Cannot delete admitted patient", WebId = "rule01" };
            rule.RuleCollection.Add(new Rule { Left = new DocumentField { Path = "Status" }, Operator = Operator.Neq, Right = new ConstantField { Value = "Admitter", Type = typeof(string) } });
            ed.BusinessRuleCollection.Add(rule);
            delete.Rules.Add(rule.Name);

            var patient = this.CreateInstance(ed, true);
            Assert.NotNull(patient);

            var cr = await delete.CompileAsync(ed);
            Assert.True(cr.Result, cr.ToString());

            var dll = Assembly.LoadFrom(cr.Output);

            var controllerType = dll.GetType($"{delete.CodeNamespace}.{delete.Name}Controller");
            Assert.NotNull(controllerType);

            var remove = controllerType.GetMethod($"Delete{delete.Name}");
            Assert.NotNull(remove);

            var httpDelete = remove.CustomAttributes.SingleOrDefault(x => x.AttributeType == typeof(HttpDeleteAttribute));
            Assert.NotNull(httpDelete);

            var parameters = remove.GetParameters();
            Assert.Equal(3, parameters.Length);
            var id = parameters[2];
            Assert.Equal("id", id.Name);
            Assert.Equal(typeof(string), id.ParameterType);
        }


        private dynamic AddMockRespository(Type edType)
        {
            var mock = typeof(ReadonlyRepository<>);
            var reposType = mock.MakeGenericType(edType);
            var repository = Activator.CreateInstance(reposType);

            var ff = typeof(IReadonlyRepository<>).MakeGenericType(edType);

            ObjectBuilder.AddCacheList(ff, repository);

            return repository;
        }

        [Fact]
        [Trait("Verb", "PATCH")]
        public async Task ConflictDetection()
        {
            var mortuary = new OperationEndpoint
            {
                Name = "SendToMortuary",
                Resource = "patients",
                Entity = "Patient",
                Route = "{id:guid}/release-with-conflict-detection",
                IsHttpPatch = true,
                WebId = "1",
                Id = "send-to-moruary",
                IsConflictDetectionEnabled = true
            };

            mortuary.PatchPathCollection.Add(new PatchSetter { Path = "DeathDateTime", IsRequired = true });

            var ed = this.CreatePatientDefinition(mortuary.Entity);
            string jsonPath = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(EntityDefinition)}\\{ed.Id}.json";
            string oePath = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(OperationEndpoint)}\\{mortuary.Id}.json";
            File.WriteAllText(jsonPath, ed.ToJsonString(true));
            File.WriteAllText(oePath, mortuary.ToJsonString(true));

            var patient = this.CreateInstance(ed, true);
            Assert.NotNull(patient);
            patient.Id = "0142ae18-a205-4979-9218-39f92b41589e";
            patient.DeathDateTime = DateTime.Today.AddDays(1);

            var cr = await mortuary.CompileAsync(ed);
            Assert.True(cr.Result, cr.ToString());

            Type patientType = patient.GetType();
            var oedll = Assembly.LoadFrom(cr.Output);

            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var controllerType = oedll.GetType($"{mortuary.CodeNamespace}.{mortuary.Name}Controller");
            dynamic controller = Activator.CreateInstance(controllerType);

            var result = await controller.PatchSendToMortuary(
                ed,
                mortuary,
                patient.Id, JsonConvert.SerializeObject(patient), new ETag("\"45\""), null);
            Assert.NotNull(result);
            Assert.IsType<InvalidResult>(result);
        }

        [Fact]
        [Trait("Verb", "PATCH")]
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
            Assert.NotNull(patient);
            patient.Id = Guid.NewGuid().ToString();
            patient.DeathDateTime = DateTime.Today.AddDays(1);


            var cr = await release.CompileAsync(ed);

            Type patientType = patient.GetType();
            var oedll = Assembly.LoadFrom(cr.Output);

            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var controllerType = oedll.GetType($"{release.CodeNamespace}.{release.Name}Controller");
            dynamic controller = Activator.CreateInstance(controllerType);

            var result = await controller.PatchRelease(
                ed,
                release,
                patient.Id, JsonSerializerService.ToJsonString(patient, true));
            Console.WriteLine("Result type : " + result);
            Assert.IsType<InvalidResult>(result);


            File.Delete(jsonPath);
            File.Delete(oePath);

        }
        [Fact]
        [Trait("Verb", "PATCH")]
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
            Assert.NotNull(patient);
            patient.DeathDateTime = DateTime.Today.AddDays(1);
            patient.Id = Guid.NewGuid().ToString();


            Type patientType = patient.GetType();
            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var cr = await release.CompileAsync(ed);
            Assert.True(cr.Result, cr.ToString());
            var opDll = Assembly.LoadFrom(cr.Output);

            var controller = opDll.CreateApiController(release.CodeNamespace + ".ReleaseController");
            var result = await controller.PatchRelease(
                ed,
                release,
                patient.Id, JsonSerializerService.ToJsonString(patient, true));
            Console.WriteLine("Result type : " + result);
            Assert.NotNull(result);

            Assert.Equal("Released", patient.Status);

            File.Delete(oePath);
            File.Delete(edPath);

        }
    }
}
