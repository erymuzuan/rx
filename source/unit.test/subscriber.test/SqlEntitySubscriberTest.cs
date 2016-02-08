using System.Linq;
using Bespoke.Sph.Domain;
using subscriber.entities;
using Xunit;

namespace subscriber.test
{
    public class SqlEntitySubscriberTest
    {
        [Fact]
        public void GenerateColumn()
        {
            var ent = new EntityDefinition { Name = "Customer", Plural = "Customers" };
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Name",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            }); ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            var address = new ComplexMember { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new SimpleMember { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);
            var sql = new SqlTableSubscriber();
            var columns = sql.GetFilterableMembers("", ent.MemberCollection).ToList();

            Assert.All(columns, Assert.NotNull);
            Assert.True(columns.Any(c => c.FullName == "Address.State"));
        }
    }
}
