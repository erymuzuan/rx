using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using NUnit.Framework;

namespace domain.test.entities
{
    [TestFixture]
    public class EntityDefinitionCodeTest
    {
        [Test]
        public void GenerateRootClass()
        {
            var ent = new EntityDefinition { Name = "Customer", Plural = "Customers" ,RecordName = "Name2"};
            ent.MemberCollection.Add(new Member
            {
                Name = "Name2",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            }); ent.MemberCollection.Add(new Member
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            var address = new Member { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new Member { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new Member { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);
            var options = new CompilerOptions
            {
                IsVerbose = true,
                IsDebug = true
            };

            var contacts = new Member { Name = "ContactCollection", Type = typeof(Array) };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ent.MemberCollection.Add(contacts);

            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\core.sph.dll")));
            options.ReferencedAssemblies.Add(Assembly.LoadFrom(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll")));


            var result = ent.Compile(options);
            result.Errors.ForEach(Console.WriteLine);
           
            Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));


        }
        [Test]
        public void GetMembersPath()
        {
            var ent = new EntityDefinition { Name = "Customer", Plural = "Customers" };
            ent.MemberCollection.Add(new Member
            {
                Name = "Name2",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            }); ent.MemberCollection.Add(new Member
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            var address = new Member { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new Member { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new Member { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);


            var contacts = new Member { Name = "ContactCollection", Type = typeof(Array) };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            contacts.MemberCollection.Add(address);
            ent.MemberCollection.Add(contacts);



            var paths = ent.GetMembersPath();
            CollectionAssert.Contains(paths, "Name2");
            CollectionAssert.Contains(paths, "Address.State");
            CollectionAssert.Contains(paths, "Contact.Name");
            CollectionAssert.Contains(paths, "Contact.Address.State");


        }
    }
}
