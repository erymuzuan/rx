using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.SqlReportDataSource;
using Xunit;

namespace domain.test.reports
{
    
    public class ReportColumnsTestFixture
    {
        private readonly MockRepository<EntityDefinition> m_efMock;

        public ReportColumnsTestFixture()
        {
            m_efMock = new MockRepository<EntityDefinition>();
            ObjectBuilder.AddCacheList<QueryProvider>(new MockQueryProvider());
            ObjectBuilder.AddCacheList<IRepository<EntityDefinition>>(m_efMock);
        }
        [Fact]
        public async Task GetColumns()
        {
            var ent = new EntityDefinition { Name = "Customer", Plural = "Customers" };
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Name2",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            }); ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = false
            });
            var address = new SimpleMember { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new SimpleMember { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);


            var contacts = new SimpleMember { Name = "ContactCollection", Type = typeof(Array) };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            contacts.MemberCollection.Add(address);
            ent.MemberCollection.Add(contacts);



            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ent);
            var ds = new SqlDataSource();
            var columns = (await ds.GetColumnsAsync("Customer"))
                .Where(x => x.IsFilterable)
                .Select(x => x.Name).ToArray();

            Assert.Contains( "Name2", columns);
            Assert.Contains("Address.State", columns);
            Assert.DoesNotContain("Title", columns);

        }
    }
}
