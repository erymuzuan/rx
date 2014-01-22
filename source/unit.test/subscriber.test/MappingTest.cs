using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using NUnit.Framework;
using subscriber.entities;

namespace subscriber.test
{
    [TestFixture]
    public class MappingTest
    {

        [Test]
        public void GenerateColumn()
        {
            var ent = new EntityDefinition { Name = "Customer", Plural = "Customers" };
            ent.MemberCollection.Add(new Member
            {
                Name = "Name",
                TypeName = "System.String, mscorlib",
                IsFilterable = true,
                IsAnalyzed = true
            }); 
            ent.MemberCollection.Add(new Member
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = true,
                IsAnalyzed = false
            });
            ent.MemberCollection.Add(new Member
            {
                Name = "RegisteredDate",
                TypeName = "System.DateTime, mscorlib",
                IsFilterable = true
            });
            var address = new Member { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new Member { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" ,IsAnalyzed = true});
            address.MemberCollection.Add(new Member { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);
           

            var locality = new Member {Name = "Locality", Type = typeof (object)};
            locality.Add(new Dictionary<string, Type>{{"Mode",typeof(string)}});
            address.MemberCollection.Add(locality);

            var sub = new EntityIndexerMappingSubscriber();
            var map = sub.GetMapping(ent);
            Console.WriteLine(map);

        }
    }
}
