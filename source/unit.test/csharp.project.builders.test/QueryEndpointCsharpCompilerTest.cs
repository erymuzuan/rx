using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Csharp.CompilersServices;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Tests.Extensions;
using Bespoke.Sph.Tests.Mocks;
using Bespoke.Sph.WebApi;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests
{
    public class QueryEndpointCsharpCompilerTest
    {
        public ITestOutputHelper Console { get; }
        private readonly Mock<ICacheManager> m_cache;
        private MockSourceRepository SourceRepository { get; } = new MockSourceRepository();


        public QueryEndpointCsharpCompilerTest(ITestOutputHelper console)
        {
            Console = console;

            m_cache = new Mock<ICacheManager>(MockBehavior.Strict);
            var git = new Mock<ICvsProvider>(MockBehavior.Strict);
            var logs = new LoadOperation<CommitLog>
            {
                CurrentPage = 1,
                PageSize = 1,
                TotalRows = 15
            };
            logs.ItemCollection.Add(new CommitLog { DateTime = DateTime.Today, Comment = "Test", CommitId = "abc123" });
            git.Setup(x => x.GetCommitLogsAsync(It.IsAny<string>(), 1, 1))
                .Callback((string file, int skip, int top) => Console.WriteLine("GetCommitLog " + file))
                .Returns(Task.FromResult(logs));
            git.Setup(x => x.GetCommitIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult("abc123"));

            ObjectBuilder.AddCacheList<ISourceRepository>(SourceRepository);
            ObjectBuilder.AddCacheList(m_cache.Object);
            ObjectBuilder.AddCacheList(git.Object);
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
        }

        private static T GetFromEmbeddedResource<T>(string resource) where T : Entity
        {
            var assembly = typeof(QueryEndpointCsharpCompilerTest).Assembly;
            var resourceName = $"Bespoke.Sph.Tests.{resource}.json";
            var stream = assembly.GetManifestResourceStream(resourceName);
            if (null != stream)
            {
                stream.Position = 0;
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    var json = reader.ReadToEnd();
                    return json.DeserializeFromJson<T>();
                }
            }

            return null;
        }
        [Fact]
        public async Task CompileQueryWithChildren()
        {
            var appointment = GetFromEmbeddedResource<EntityDefinition>("Appointment");
            var query = new QueryEndpoint
            {
                Name = "Appointment for patient",
                Route = "~/api/patients/{mrn}/appointments",
                Id = "appointments-for-patient",
                Entity = "Appointment",
                Resource = "appointments",
                CacheFilter = 300
            };

            var mrnParameter = new RouteParameterField { Name = "mrn", Type = typeof(string), WebId = "mrn" };
            var start = new RouteParameterField { Name = "start", Type = typeof(DateTime), IsOptional = true, DefaultValue = "2016-01-01", WebId = "start" };
            var end = new RouteParameterField { Name = "end", Type = typeof(DateTime), IsOptional = true, DefaultValue = "2017-01-01", WebId = "end" };
            query.FilterCollection.Add(new Filter
            {
                Field = mrnParameter,
                Operator = Operator.Eq,
                Term = "Mrn"
            });
            query.FilterCollection.Add(new Filter
            {
                Field = start,
                Operator = Operator.Ge,
                Term = "DateTime"
            });
            query.FilterCollection.Add(new Filter
            {
                Field = end,
                Operator = Operator.Lt,
                Term = "DateTime"
            });

            query.MemberCollection.AddRange("Id", "ReferenceNo", "DateTime", "Doctor", "Location", "Ward");

            var compiler = new QueryEndpointCompiler();
            var result = await compiler.BuildAsync(query, x => new CompilerOptions2());

            Assert.True(result.Result, result.ToString());

        }





        private dynamic AddMockRespository(Type edType)
        {
            var mock = typeof(MockReadOnlyRepository<>);
            var reposType = mock.MakeGenericType(edType);
            var repository = Activator.CreateInstance(reposType);

            var ff = typeof(IReadOnlyRepository<>).MakeGenericType(edType);

            ObjectBuilder.AddCacheList(ff, repository);

            return repository;
        }

        [Fact]
        public void GeneratePatientTest()
        {
            var ed = this.CreatePatientDefinition();
            var patient = ed.CreateInstanceAsync(true);
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
            Assert.NotEmpty(roles);
            var endpoint = new OperationEndpoint
            {
                Name = name,
                Resource = "patients",
                Entity = "Patient" + name,
                IsHttpPost = true,
                WebId = "abc",
                Id = "patient-register"
            };
            endpoint.AddRules("VerifyRegisteredDate");

            var ed = this.CreatePatientDefinition(endpoint.Entity);
            var patient = ed.CreateInstanceAsync(true);
            Assert.NotNull(patient);

            m_cache.Setup(x => x.Get<EntityDefinition>(ed.Id))
                .Returns(ed);

            var compiler = new OperationEndpointCompiler();
            var result = await compiler.BuildAsync(endpoint, x => new CompilerOptions2());
            Assert.True(result.Result, result.ToString());

            try
            {
                var dll = Assembly.LoadFrom(result.Output);

                var controllerType = dll.GetType($"{endpoint.CodeNamespace}.{endpoint.TypeName}");
                var action = controllerType.GetMethod("Post" + name);
                var controller = Activator.CreateInstance(controllerType);
                var bs = controller as BaseApiController;
                Assert.NotNull(bs);
                bs.Configuration = new HttpConfiguration();

                Assert.NotNull(action);

                var awaiter = (Task<IHttpActionResult>)action.Invoke(controller, new object[] { ed, endpoint, patient });
                var response = (AcceptedResult)await awaiter;
                var json = JObject.Parse(JsonConvert.SerializeObject(response.Result));

                var id = json.SelectToken("$.id").Value<string>();
                var location = json.SelectToken("$._links.href").Value<string>();
                Assert.Equal(location, $"{ConfigurationManager.BaseUrl}/api/patients/" + id);


                // check header location
                Assert.Equal(location, response.LocationUri.ToString());

                // 202
                Assert.NotNull(response);
                Assert.NotNull(response.GetType());
                var responseTypeName = response.GetType().FullName;
                Assert.NotNull(responseTypeName);
                Assert.Contains("Accepted", responseTypeName);
            }
            catch (ReflectionTypeLoadException e)
            {
                Console.WriteLine(e.ToString());
                foreach (var inner in e.LoaderExceptions)
                {
                    Console.WriteLine(inner.ToString());
                }

                throw;
            }
        }

        [Fact]
        [Trait("Verb", "PATCH")]
        public async Task HttpPatchReleaseOperation()
        {
            var ed = this.CreatePatientDefinition("PatientForRelease");
            var patient = ed.CreateInstanceAsync(true);
            Assert.NotNull(patient);


            var release = new OperationEndpoint { Name = "Release", Entity = ed.Name, Resource = "patients", IsHttpPatch = true, WebId = "ReleaseWithPatch" };
            release.PatchPathCollection.Add(new PatchSetter { Path = "Status", DefaultValue = "\"Released\"" });
            //release.PatchPathCollection.Add("ClinicalNote");
            release.Rules.Add("VerifyRegisteredDate");

            var compiler = new OperationEndpointCompiler();
            var cr = await compiler.BuildAsync(release, x => new CompilerOptions2());
            Assert.True(cr.Result, cr.ToString());


            var dll = Assembly.LoadFrom(cr.Output);
            var controllerType = dll.GetType($"{release.CodeNamespace}.{release.TypeName}");
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


            var patient = ed.CreateInstanceAsync(true);
            Assert.NotNull(patient);

            var compiler = new OperationEndpointCompiler();
            var cr = await compiler.BuildAsync(admit, x => new CompilerOptions2());
            Assert.True(cr.Result, cr.ToString());

            var dll = Assembly.LoadFrom(cr.Output);
            var controllerType = dll.GetType($"{admit.CodeNamespace}.{admit.TypeName}");
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

            var patient = ed.CreateInstanceAsync(true);
            Assert.NotNull(patient);

            var compiler = new OperationEndpointCompiler();
            var cr = await compiler.BuildAsync(admit, x => new CompilerOptions2());
            Assert.True(cr.Result, cr.ToString());

            var dll = Assembly.LoadFrom(cr.Output);
            var controllerType = dll.GetType($"{admit.CodeNamespace}.{admit.TypeName}");
            Assert.NotNull(controllerType);

            var releaseActionMethodInfo = controllerType.GetMethod($"Put{admit.Name}");
            Assert.NotNull(releaseActionMethodInfo);

        }


        [Fact]
        [Trait("Verb", "DELETE")]
        public async Task HttpDelete()
        {
            var ed = this.CreatePatientDefinition("PatientDelete");
            var patient = await ed.CreateInstanceAsync(true);
            Assert.NotNull(patient);
            patient.FullName = "Azman";
            Assert.Equal("Azman", patient.FullName);

            SourceRepository.AddOrReplace(ed);

            var delete = new OperationEndpoint { Name = "Remove", Id = "delete-patient", Entity = ed.Name, Route = "", IsHttpDelete = true, WebId = "remove" };

            var cr = await delete.CreateInstanceAsync();
            Type controllerType = cr.GetType();;
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

            var patient = ed.CreateInstanceAsync(true);
            Assert.NotNull(patient);



            var compiler = new OperationEndpointCompiler();
            var cr = await compiler.BuildAsync(delete, x => new CompilerOptions2());

            Assert.True(cr.Result, cr.ToString());

            var dll = Assembly.LoadFrom(cr.Output);

            var controllerType = dll.GetType($"{delete.CodeNamespace}.{delete.TypeName}");
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

            var patient = await ed.CreateInstanceAsync(true);
            Assert.NotNull(patient);
            patient.Id = "0142ae18-a205-4979-9218-39f92b41589e";
            patient.DeathDateTime = DateTime.Today.AddDays(1);



            var compiler = new OperationEndpointCompiler();
            var cr = await compiler.BuildAsync(mortuary, x => new CompilerOptions2());

            Assert.True(cr.Result, cr.ToString());

            Type patientType = patient.GetType();
            var oedll = Assembly.LoadFrom(cr.Output);

            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var controllerType = oedll.GetType($"{mortuary.CodeNamespace}.{mortuary.TypeName}");
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
                Left = new DocumentField { Path = "DeathDateTime", Type = typeof(DateTime?) },
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
            var jsonPath = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(EntityDefinition)}\\{ed.Id}.json";
            var oePath = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(OperationEndpoint)}\\{release.Id}.json";
            File.WriteAllText(jsonPath, ed.ToJsonString(true));
            File.WriteAllText(oePath, release.ToJsonString(true));

            var patient = await ed.CreateInstanceAsync(true);
            Assert.NotNull(patient);
            patient.Id = Guid.NewGuid().ToString();
            patient.DeathDateTime = DateTime.Today.AddDays(1);


            var compiler = new OperationEndpointCompiler();
            var cr = await compiler.BuildAsync(release, x => new CompilerOptions2());

            Type patientType = patient.GetType();
            var oedll = Assembly.LoadFrom(cr.Output);

            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var controllerType = oedll.GetType($"{release.CodeNamespace}.{release.TypeName}");
            dynamic controller = Activator.CreateInstance(controllerType);

            var result = await controller.PatchRelease(
                ed,
                release,
                patient.Id, JsonConvert.SerializeObject(patient));
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


            var patient = await ed.CreateInstanceAsync(true);
            Assert.NotNull(patient);
            patient.DeathDateTime = DateTime.Today.AddDays(1);
            patient.Id = Guid.NewGuid().ToString();


            Type patientType = patient.GetType();
            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var compiler = new OperationEndpointCompiler();
            var cr = await compiler.BuildAsync(release, x => new CompilerOptions2());
            Assert.True(cr.Result, cr.ToString());
            var opDll = Assembly.LoadFrom(cr.Output);

            var controller = opDll.CreateApiController(release.CodeNamespace + "." + release.TypeName);
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

        [Fact]
        [Trait("Verb", "PATCH")]
        public async Task ConflictDetection2()
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
            var jsonPath = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(EntityDefinition)}\\{ed.Id}.json";
            var oePath = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(OperationEndpoint)}\\{mortuary.Id}.json";
            File.WriteAllText(jsonPath, ed.ToJsonString(true));
            File.WriteAllText(oePath, mortuary.ToJsonString(true));

            var patient = await ed.CreateInstanceAsync(true);
            Assert.NotNull(patient);
            patient.Id = "0142ae18-a205-4979-9218-39f92b41589e";
            patient.DeathDateTime = DateTime.Today.AddDays(1);

            var compiler = new OperationEndpointCompiler();
            var cr = await compiler.BuildAsync(mortuary, x => new CompilerOptions2());
            Assert.True(cr.Result, cr.ToString());

            Type patientType = patient.GetType();
            var oedll = Assembly.LoadFrom(cr.Output);

            var repos = AddMockRespository(patientType);
            repos.AddToDictionary(patient.Id, patient);

            var controllerType = oedll.GetType($"{mortuary.CodeNamespace}.{mortuary.TypeName}");
            dynamic controller = Activator.CreateInstance(controllerType);

            var result = await controller.PatchSendToMortuary(
                ed,
                mortuary,
                patient.Id, JsonConvert.SerializeObject(patient), new ETag("\"45\""), null);
            Assert.NotNull(result);
            Assert.IsType<InvalidResult>(result);
        }


        public EntityDefinition CreatePatientDefinition(string name = "TestingPatient")
        {
            var ent = new EntityDefinition { Name = name, Plural = "Patients", RecordName = "FullName", Id = "patient" };
            ent.AddSimpleMember<string>("FullName");
            ent.AddSimpleMember<string>("Status");
            ent.AddSimpleMember<string>("Title");
            ent.AddSimpleMember<string>("DeathDateTime");
            ent.AddSimpleMember<string>("Discharged");
            var address = new ComplexMember { Name = "HomeAddress", TypeName = "Address" };
            address.AddMember<string>("Street1");
            address.AddMember<string>("State");
            ent.MemberCollection.Add(address);


            var contacts = new ComplexMember { Name = "Contacts", TypeName = "Contact", AllowMultiple = true };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ent.MemberCollection.Add(contacts);

            return ent;

        }

    }
}
