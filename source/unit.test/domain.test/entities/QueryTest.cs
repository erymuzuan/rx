using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using domain.test.reports;
using domain.test.triggers;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.entities
{
    [Trait("Category", "Query endpoints")]
    [Collection("Endpoint")]
    public class QueryTest
    {
        private readonly ITestOutputHelper m_testOutput;
        private readonly MockPersistence m_persistence = new MockPersistence();

        public QueryTest(ITestOutputHelper testOutput)
        {
            m_testOutput = testOutput;
            var efMock = new MockRepository<EntityDefinition>();
            efMock.AddToDictionary("", GetFromEmbeddedResource<EntityDefinition>("Patient"));
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(efMock);
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher());
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<IPersistence>(m_persistence);
            m_testOutput.WriteLine("Init..");
        }

        [Fact]
        [Trait("Category", "Compile")]
        public void GenerateController()
        {
            var query = new QueryEndpoint { Name = "All patients", Route = "all-patients", Id = "all-patients", Entity = "Patient", WebId = "all-patients" };
            var patient = GetFromEmbeddedResource<EntityDefinition>(query.Entity);
            var options = new CompilerOptions();
            var sources = query.GenerateCode(patient);
            var result = query.Compile(options, sources);

            m_testOutput.WriteLine(result.ToString());

            Assert.True(result.Result, result.ToString());
        }

        public static IEnumerable<object[]> Filters
        {
            get
            {
                yield return new object[] {
                    GetFromEmbeddedResource<EntityDefinition>("Patient"),

                    new QueryEndpoint
                    {
                        Name = "Patients died this week",
                        Route = "~/api/patients/patients-died-this-week",
                        Id = "patients-died-this-week",
                        Entity = "Patient",
                        WebId = "all-patients"
                    },
                new []
                    {
                        new Filter
                        {
                            Field = new FunctionField { Script = "DateTime.Today.AddDays(-7)" },
                            Operator = Operator.Ge,
                            Term = "DeathDate"
                        }
                    }
                };
            }
        }

        [Theory]
        [MemberData("Filters")]
        public void Compile(EntityDefinition ed, QueryEndpoint query, Filter[] filters)
        {
            query.FilterCollection.AddRange(filters);

            var options = new CompilerOptions();
            var sources = query.GenerateCode(ed);
            var result = query.Compile(options, sources);

            Assert.True(result.Result, result.ToString());

            var output = $"{query.AssemblyName}".Replace(".dll","");
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.dll", $"{ConfigurationManager.WebPath}\\bin\\{output}.dll", true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.pdb", $"{ConfigurationManager.WebPath}\\bin\\{output}.pdb", true);


            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\{nameof(QueryEndpoint)}\\{query.Id}.json", query.ToJsonString(true));
        }

        [Theory]
        [InlineData("AnonymousQuery")]
        [InlineData("Everybody")]
        [InlineData("DesignationU32")]
        [InlineData("RolesNurse")]
        [InlineData("UserNameAli")]
        public async Task CompileWithPerformer(string name)
        {
            var query = new QueryEndpoint
            {
                Name = name,
                Route = "~/api/patients/" + name.ToIdFormat(),
                Id = name.ToIdFormat(),
                Entity = "Patient",
                WebId = Guid.NewGuid().ToString()
            };
            var ed = GetFromEmbeddedResource<EntityDefinition>("Patient");
            var result = await query.CompileAsync(ed);

            Assert.True(result.Result, result.ToString());

            var output = $"{query.AssemblyName}".Replace(".dll","");
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.dll", $"{ConfigurationManager.WebPath}\\bin\\{output}.dll", true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.pdb", $"{ConfigurationManager.WebPath}\\bin\\{output}.pdb", true);


            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\{nameof(QueryEndpoint)}\\{query.Id}.json", query.ToJsonString(true));
        }


        [Theory]
        [InlineData(null, null)]
        [InlineData("Roles", "Administrators")]
        [InlineData("Designation", "Senior Manager")]
        public async Task QueryFields(string performer, string performerValues)
        {
            var query = new QueryEndpoint
            {
                Name = "Patients Born in 60s",
                Route = "~/api/patients/born-in-60s",
                Id = "patients-born-in-60s",
                Entity = "Patient",
                WebId = "all-born-in-60s"
            };
            var fields = new[] { "Dob", "FullName", "Gender", "Race" };
            query.MemberCollection.AddRange(fields);


            var json = await query.GenerateEsQueryAsync();
            var jo = JObject.Parse(json);
            var esFields = jo.SelectToken("$.fields").Values<string>().ToArrayString();

            Assert.Equal(fields, esFields);
        }

        [Fact]
        [Trait("Query", "Elasticsearch")]
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
            var fields = jo.SelectToken("$.fields").Values<string>().ToArray();
            Assert.Contains("Dob", fields);
        }

        [Fact]
        public void CompileQueryWithFieldsAndFilter()
        {
            var patient = GetFromEmbeddedResource<EntityDefinition>("Patient");
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

            Assert.True(result.Result, result.ToString());


            var output = $"{query.AssemblyName}".Replace(".dll", "");
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.dll", $"{ConfigurationManager.WebPath}\\bin\\{output}.dll", true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.pdb", $"{ConfigurationManager.WebPath}\\bin\\{output}.pdb", true);

            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\{nameof(QueryEndpoint)}\\{query.Id}.json", query.ToJsonString(true));
        }

        [Fact]
        public void CompileQueryWithFieldsAndFilterCacheFilter()
        {
            var patient = GetFromEmbeddedResource<EntityDefinition>("Patient");
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

            query.MemberCollection.AddRange("Id", "Dob", "FullName", "Gender", "Race", "DeathDate", "NextOfKin.FullName",
                "NextOfKin.Relationship", "HomeAddress.State", "Wife.Name", "Wife.WorkPlaceAddress.State");

            var options = new CompilerOptions();
            var sources = query.GenerateCode(patient);
            var result = query.Compile(options, sources);

            Assert.True(result.Result, result.ToString());


            var output = $"{query.AssemblyName}".Replace(".dll", "");
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.dll", $"{ConfigurationManager.WebPath}\\bin\\{output}.dll", true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{output}.pdb", $"{ConfigurationManager.WebPath}\\bin\\{output}.pdb", true);

            File.WriteAllText($"{ConfigurationManager.SphSourceDirectory}\\{nameof(QueryEndpoint)}\\{query.Id}.json", query.ToJsonString(true));
        }

        private static T GetFromEmbeddedResource<T>(string entityDefinitionName) where T : Entity
        {
            var assembly = typeof(QueryTest).Assembly;
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

        [Fact]
        public void CompileQueryWithChildren()
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

            Assert.True(result.Result, result.ToString());

        }
    }
}