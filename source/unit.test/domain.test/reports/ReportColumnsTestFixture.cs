using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.SqlReportDataSource;
using NUnit.Framework;

namespace domain.test.reports
{
    [TestFixture]
    public class ReportColumnsTestFixture
    {
        private MockRepository<EntityDefinition> m_efMock;

        [SetUp]
        public void Init()
        {
            m_efMock = new MockRepository<EntityDefinition>();
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(m_efMock);
        }
        [Test]
        public async Task GetColumns()
        {
            var ent = new EntityDefinition { Name = "Customer", Plural = "Customers" ,Id = "customer"};
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Name2",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = false
            });
            var address = new ComplexMember { Name = "Address" };
            address.MemberCollection.Add(new SimpleMember { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);


            var contacts = new ComplexMember { Name = "Contacts", AllowMultiple = true };
            contacts.MemberCollection.Add(new SimpleMember { Name = "Name", Type = typeof(string) });
            contacts.MemberCollection.Add(new SimpleMember { Name = "Telephone", Type = typeof(string) });
            contacts.MemberCollection.Add(address);
            ent.MemberCollection.Add(contacts);
            
            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ent);
            var ds = new SqlDataSource();
            var columns = (await ds.GetColumnsAsync("Customer"))
                .Where(x => x.IsFilterable)
                .Select(x => x.Name).ToArray();

            CollectionAssert.Contains(columns, "Name2");
            CollectionAssert.Contains(columns, "Address.State");
            CollectionAssert.DoesNotContain(columns, "Title");

        }
    }
}
