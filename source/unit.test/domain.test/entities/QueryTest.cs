using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using domain.test.mocks;
using domain.test.reports;
using domain.test.triggers;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.entities
{
    [Trait("Category", "Query endpoints")]
    [Collection("Endpoint")]
    public class QueryTest
    {
        public QueryTest(ITestOutputHelper console)
        {
            var persistence = new MockPersistence(console);
            var efMock = new MockRepository<EntityDefinition>();
            efMock.AddToDictionary("", GetFromEmbeddedResource<EntityDefinition>("Patient"));
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(efMock);
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher(console));
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<IPersistence>(persistence);
            ObjectBuilder.AddCacheList<ICvsProvider>(new MockCvsProvider(new[]
            {
                new CommitLog
                {
                    Files = new []{"a.cs" ,"b"},
                    CommitId = "1",
                    Comment = "One",
                    DateTime = DateTime.Today.AddHours(1)
                },
                new CommitLog
                {
                    Files = new []{"a.cs" ,"b"},
                    CommitId = "2",
                    Comment = "Two",
                    DateTime = DateTime.Today.AddHours(2)
                }
            }));

            console.WriteLine("Init..");
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

    }
}