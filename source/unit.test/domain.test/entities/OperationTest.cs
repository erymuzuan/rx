using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using domain.test.Extensions;
using domain.test.reports;
using domain.test.triggers;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace domain.test.entities
{
    [Trait("Category", "Operation endpoints")]
    [Trait("Category", "Compile")]
    public class OperationTest
    {
        private readonly ITestOutputHelper m_output;
        private readonly Mock<ICacheManager> m_cache;

        public OperationTest(ITestOutputHelper output)
        {
            m_output = output;
            var persistence = new MockPersistence(output);
            var efMock = new MockRepository<EntityDefinition>();
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(efMock);
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new MockChangePublisher(output));
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
            ObjectBuilder.AddCacheList<IPersistence>(persistence);

            m_cache = new Mock<ICacheManager>();
            m_cache.Setup(x => x.Get<QueryEndpoint>(It.IsAny<string>()))
                .Returns(new QueryEndpoint());
            ObjectBuilder.AddCacheList(m_cache.Object);
        }
        private async Task<dynamic> CreateInstanceAsync(EntityDefinition ed, bool verbose = false)
        {
            var core = Path.GetFullPath($@"{ConfigurationManager.WebPath}\bin\core.sph.dll");

            var destinationCore = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "core.sph.dll");
            File.Copy(core, destinationCore, true);
            
            var result = await ed.CompileWithCsharpAsync();
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


        private dynamic AddMockRespository(Type edType)
        {
            var mock = typeof(MockReadOnlyRepository<>);
            var reposType = mock.MakeGenericType(edType);
            var repository = Activator.CreateInstance(reposType);

            var ff = typeof(IReadOnlyRepository<>).MakeGenericType(edType);

            ObjectBuilder.AddCacheList(ff, repository);

            return repository;
        }

    }
}
