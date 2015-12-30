using System.Linq;
using Bespoke.Sph.Domain;
using NUnit.Framework;
using subscriber.entities;

namespace subscriber.test
{
    [TestFixture]
    public class SqlEntitySubscriberTest
    {
        [Test]
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
            var address = new SimpleMember { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new SimpleMember { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib"});
            address.MemberCollection.Add(new SimpleMember { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib"});
            ent.MemberCollection.Add(address);
            var sql = new SqlTableSubscriber();
            var columns = sql.GetFilterableMembers("", ent.MemberCollection).ToList();
            
            CollectionAssert.AllItemsAreNotNull(columns);
            Assert.IsTrue(columns.Any(c => c.FullName == "Address.State"));
        }
    }
}
