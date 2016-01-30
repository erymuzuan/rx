using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using domain.test.reports;
using domain.test.triggers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace domain.test.entities
{
    [TestFixture]
    public class QueryTest
    {
        private MockRepository<EntityDefinition> m_efMock;
        private readonly MockPersistence m_persistence = new MockPersistence();

        [SetUp]
        public void Setup()
        {
            m_efMock = new MockRepository<EntityDefinition>();
            m_efMock.AddToDictionary("", this.GetFromEmbeddedResource<EntityDefinition>("Patient"));
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(m_efMock);
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<IPersistence>(m_persistence);
        }

        [Test]
        public void GenerateController()
        {
            var query = new QueryEndpoint { Name = "All patients", Route = "all-patients", Id = "all-patients", Entity = "Patient", WebId = "all-patients" };
            var patient = this.GetFromEmbeddedResource<EntityDefinition>(query.Entity);
            var options = new CompilerOptions();
            var sources = query.GenerateCode(patient);
            var result = query.Compile(options, sources);

            Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));
        }


        [Test]
        public void CompileQueryWithFunctionFilter()
        {
            var patient = this.GetFromEmbeddedResource<EntityDefinition>("Patient");
            var query = new QueryEndpoint
            {
                Name = "Patients died this week",
                Route = "~/api/patients/patients-died-this-week",
                Id = "patients-died-this-week",
                Entity = "Patient",
                WebId = "all-patients"
            };
            var lastWeek = new FunctionField { Script = "DateTime.Today.AddDays(-7)" };
            query.FilterCollection.Add(new Filter
            {
                Field = lastWeek,
                Operator = Operator.Ge,
                Term = "DeathDate"
            });

            var options = new CompilerOptions();
            var sources = query.GenerateCode(patient);
            var result = query.Compile(options, sources);

            Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));


            var output = $"{ConfigurationManager.ApplicationName}.EntityQuery.{query.Id}";
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.dll", $"{ConfigurationManager.WebPath}\\bin\\{output}.dll", true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.pdb", $"{ConfigurationManager.WebPath}\\bin\\{output}.pdb", true);


            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\EntityQuery\\{query.Id}.json", query.ToJsonString(true));
        }


        [Test]
        public async Task QueryFields()
        {
            var query = new QueryEndpoint
            {
                Name = "Patients Born in 60s",
                Route = "~/api/patients/born-in-60s",
                Id = "patients-born-in-60s",
                Entity = "Patient",
                WebId = "all-born-in-60s"
            };
            query.MemberCollection.AddRange("Dob", "FullName", "Gender", "Race");

            var json = await query.GenerateEsQueryAsync();
            var jo = JObject.Parse(json);
            Assert.AreEqual(new[] { "Dob", "FullName", "Gender", "Race" }, jo.SelectToken("$.fields").Values<string>().ToArrayString(), jo.ToString());
        }


        public async Task CompileQueryFieldsAndFilter()
        {
            var query = new QueryEndpoint
            {
                Name = "Patients Born in 60s",
                Route = "~/api/patients/born-in-60s",
                Id = "patients-born-in-60s",
                Entity = "Patient",
                WebId = "all-born-in-60s"
            };
            var sixty = new ConstantField { Type = typeof(DateTime), Value = "1960-01-01" };
            var sixtyNine = new ConstantField { Type = typeof(DateTime), Value = "1969-12-31" };
            query.FilterCollection.Add(new Filter
            {
                Field = sixty,
                Operator = Operator.Ge,
                Term = "Dob"
            });
            query.FilterCollection.Add(new Filter
            {
                Field = sixtyNine,
                Operator = Operator.Le,
                Term = "Dob"
            });

            query.MemberCollection.AddRange("Dob", "FullName", "Gender", "Race");

            var json = await query.GenerateEsQueryAsync();
            var jo = JObject.Parse(json);
            Assert.AreEqual(120, jo.SelectToken("$.from").Value<int>(), jo.ToString());
            Assert.AreEqual(30, jo.SelectToken("$.size").Value<int>(), jo.ToString());
        }

        [Test]
        public void CompileQueryWithFieldsAndFilter()
        {
            var patient = this.GetFromEmbeddedResource<EntityDefinition>("Patient");
            var query = new QueryEndpoint
            {
                Name = "Patients Born in 60s",
                Route = "~/api/patients/born-in-60s",
                Id = "patients-born-in-60s",
                Entity = patient.Name,
                WebId = "all-born-in-60s"
            };
            var sixty = new ConstantField { Type = typeof(DateTime), Value = "1960-01-01" };
            var sixtyNine = new ConstantField { Type = typeof(DateTime), Value = "1969-12-31" };
            query.FilterCollection.Add(new Filter
            {
                Field = sixty,
                Operator = Operator.Ge,
                Term = "Dob"
            });
            query.FilterCollection.Add(new Filter
            {
                Field = sixtyNine,
                Operator = Operator.Le,
                Term = "Dob"
            });

            query.MemberCollection.AddRange("Dob", "FullName", "Gender", "Race", "DeathDate");

            var options = new CompilerOptions();
            var sources = query.GenerateCode(patient);
            var result = query.Compile(options, sources);

            Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));


            var output = $"{ConfigurationManager.ApplicationName}.EntityQuery.{query.Id}";
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.dll", $"{ConfigurationManager.WebPath}\\bin\\{output}.dll", true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.pdb", $"{ConfigurationManager.WebPath}\\bin\\{output}.pdb", true);

            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\EntityQuery\\{query.Id}.json", query.ToJsonString(true));
        }

        [Test]
        public void CompileQueryWithFieldsAndFilterCacheFilter()
        {
            var patient = this.GetFromEmbeddedResource<EntityDefinition>("Patient");
            var query = new QueryEndpoint
            {
                Name = "Patients with cached filter",
                Route = "~/api/patients/cache-filter",
                Id = "patients-cache-filter",
                Entity = "Patient",
                WebId = "patients-cache-filter",
                Resource = "patients",
                CacheFilter = 300
            };
            var sixty = new ConstantField { Type = typeof(DateTime), Value = "1960-01-01" };
            var sixtyNine = new ConstantField { Type = typeof(DateTime), Value = "1969-12-31" };
            query.FilterCollection.Add(new Filter
            {
                Field = sixty,
                Operator = Operator.Ge,
                Term = "Dob"
            });
            query.FilterCollection.Add(new Filter
            {
                Field = sixtyNine,
                Operator = Operator.Le,
                Term = "Dob"
            });

            query.MemberCollection.AddRange("Id", "Dob", "FullName", "Gender", "Race", "DeathDate");

            var options = new CompilerOptions();
            var sources = query.GenerateCode(patient);
            var result = query.Compile(options, sources);

            Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));


            var output = $"{ConfigurationManager.ApplicationName}.EntityQuery.{query.Id}";
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.dll", $"{ConfigurationManager.WebPath}\\bin\\{output}.dll", true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.pdb", $"{ConfigurationManager.WebPath}\\bin\\{output}.pdb", true);

            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\EntityQuery\\{query.Id}.json", query.ToJsonString(true));
        }

        private T GetFromEmbeddedResource<T>(string entityDefinitionName) where T : Entity
        {
            var assembly = this.GetType().Assembly;
            var resourceName = $"domain.test.entities.{entityDefinitionName}.json";
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

        [Test]
        public void CompileQueryWithChildren()
        {
            var appointment = this.GetFromEmbeddedResource<EntityDefinition>("Appointment");
            var query = new QueryEndpoint
            {
                Name = "Appointment for patient",
                Route = "~/api/patients/{mrn}/appointments",
                Id = "appointments-for-patient",
                Entity = "Appointment",
                Resource = "appointments",
                CacheFilter = 300
            };
            query.RouteParameterCollection.Add(new RouteParameter { Type = "string", Name = "mrn" });
            query.RouteParameterCollection.Add(new RouteParameter { Type = "DateTime", Name = "start" });
            query.RouteParameterCollection.Add(new RouteParameter { Type = "DateTime", Name = "end" });

            var mrnParameter = new RouteParameterField { Expression = "mrn" };
            var start = new RouteParameterField { Expression = "start", DefaultValue = "2016-01-01" };
            var end = new RouteParameterField { Expression = "end", DefaultValue = "2017-01-01" };
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

            var options = new CompilerOptions();
            var sources = query.GenerateCode(appointment);
            var result = query.Compile(options, sources);

            Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));


            var output = $"{ConfigurationManager.ApplicationName}.EntityQuery.{query.Id}";
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.dll", $"{ConfigurationManager.WebPath}\\bin\\{output}.dll", true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.pdb", $"{ConfigurationManager.WebPath}\\bin\\{output}.pdb", true);

            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\EntityQuery\\{query.Id}.json", query.ToJsonString(true));
        }
    }
}