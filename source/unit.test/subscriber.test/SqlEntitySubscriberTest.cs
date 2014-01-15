using System;
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
            ent.MemberCollection.Add(new Member
            {
                Name = "Name",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            }); ent.MemberCollection.Add(new Member
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            var address = new Member { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new Member { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib"});
            address.MemberCollection.Add(new Member { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib"});
            ent.MemberCollection.Add(address);
            var sql = new SqlTableSubscriber();
            var columns = sql.GetFiltarableMembers("", ent.MemberCollection);
            foreach (var column in columns)
            {
                Console.WriteLine(column);
            }
        }
    }
}
