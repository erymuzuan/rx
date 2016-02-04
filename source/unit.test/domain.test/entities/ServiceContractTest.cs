using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using Moq;
using NUnit.Framework;

namespace domain.test.entities
{
    [TestFixture]
    public class ServiceContractTest
    {
        private Mock<IRepository<ValueObjectDefinition>> m_vodRepo;
        private Mock<IDirectoryService> m_ds;
        [SetUp]
        public void Init()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());

            var qp = new MockQueryProvider();
            ObjectBuilder.AddCacheList<QueryProvider>(qp);


            m_ds = new Mock<IDirectoryService>(MockBehavior.Strict);
            ObjectBuilder.AddCacheList(m_ds.Object);

            var cacheManager = new Mock<ICacheManager>();
            cacheManager.Setup(x => x.Get<ServiceContractSetting>(It.IsAny<string>()))
                .Returns(new ServiceContractSetting());
            ObjectBuilder.AddCacheList(cacheManager.Object);

            m_vodRepo = new Mock<IRepository<ValueObjectDefinition>>(MockBehavior.Strict);

            string path = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(ValueObjectDefinition)}\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            ObjectBuilder.AddCacheList(m_vodRepo.Object);

            var address = new ValueObjectDefinition { Name = "Address", Id = "address", ChangedDate = DateTime.Now, ChangedBy = "Me", CreatedBy = "Me", CreatedDate = DateTime.Now };
            address.MemberCollection.Add(new SimpleMember { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "Street2", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "Postcode", IsFilterable = true, TypeName = "System.String, mscorlib" });

            var spouse = new ValueObjectDefinition { Name = "Spouse", Id = "spouse" };
            spouse.MemberCollection.Add(new SimpleMember { Name = "Name", Type = typeof(string) });
            spouse.MemberCollection.Add(new SimpleMember { Name = "Age", Type = typeof(int) });
            spouse.MemberCollection.Add(new ValueObjectMember { Name = "WorkPlaceAddress", ValueObjectName = "Address" });

            var child = new ValueObjectDefinition { Name = "Child", Id = "child" };
            child.MemberCollection.Add(new SimpleMember { Name = "Name", Type = typeof(string) });
            child.MemberCollection.Add(new SimpleMember { Name = "Age", Type = typeof(int) });

            address.Save();
            spouse.Save();
            child.Save();
        }

        [Test]
        public async Task GenerateGetOneByIdTest()
        {
            var patient = PatientSourceJson.DeserializeFromJson<EntityDefinition>();
            patient.ServiceContract.EntityResourceEndpoint.IsAllowed = true;

            var cr = await patient.ServiceContract.CompileAsync(patient);
            Assert.IsTrue(cr.Result, cr.ToString());

        }
        [Test]
        public async Task SaveSetting()
        {
            var patient = PatientSourceJson.DeserializeFromJson<EntityDefinition>();
            var setting = await patient.ServiceContract.LoadSettingAsync(patient.Name);
            setting.ResourceEndpointSetting.CachingSetting.CacheControl = "Public";
            setting.ResourceEndpointSetting.CachingSetting.Expires = 300;

            await patient.ServiceContract.SaveSetttingAsync(setting, patient.Name);
        }


        [Test]
        public async Task GenerateSearch()
        {
            var patient = PatientSourceJson.DeserializeFromJson<EntityDefinition>();
            patient.ServiceContract.FullSearchEndpoint.IsAllowed = true;
            patient.ServiceContract.EntityResourceEndpoint.IsAllowed = true;
            patient.ServiceContract.OdataEndpoint.IsAllowed = true;


            var cr = await patient.ServiceContract.CompileAsync(patient);
            Assert.IsTrue(cr.Result, cr.ToString());
        }

        private static string PatientSourceJson => File.ReadAllText($"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\patient.json");
    }
}