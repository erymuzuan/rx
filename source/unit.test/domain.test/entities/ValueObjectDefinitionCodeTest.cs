using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using Bespoke.Sph.RoslynScriptEngines;
using domain.test.Extensions;
using Moq;
using Xunit;

namespace domain.test.entities
{
    public class ValueObjectDefinitionCodeTest
    {
        public ValueObjectDefinitionCodeTest()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());

            var qp = new MockQueryProvider();
            ObjectBuilder.AddCacheList<QueryProvider>(qp);


            var ds = new Mock<IDirectoryService>(MockBehavior.Strict);
            ObjectBuilder.AddCacheList(ds.Object);

            var vodRepo = new Mock<IRepository<ValueObjectDefinition>>(MockBehavior.Strict);

            string path = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(ValueObjectDefinition)}\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            ObjectBuilder.AddCacheList(vodRepo.Object);

            var address = new ValueObjectDefinition { Name = "Address", Id = "address", ChangedDate = DateTime.Now, ChangedBy = "Me", CreatedBy = "Me", CreatedDate = DateTime.Now };
            address.MemberCollection.Add(new SimpleMember { Name = "Street1",  TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "Street2", TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "State",  TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "Postcode", TypeName = "System.String, mscorlib" });

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

        [Fact]
        public async Task GenerateCodeBasic()
        {
            var ent = new EntityDefinition { Name = "Customer", Plural = "Customers", RecordName = "Name2" };
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Name2",
                TypeName = "System.String, mscorlib"
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Titles",
                TypeName = "System.String, mscorlib",
                AllowMultiple = true
            });


            var home = new ValueObjectMember { ValueObjectName = "Address", Name = "HomeAddress" };
            ent.MemberCollection.Add(home);

            var office = new ValueObjectMember { ValueObjectName = "Address", Name = "WorkPlaceAddress" };
            ent.MemberCollection.Add(office);
            ent.MemberCollection.Add(new ValueObjectMember { ValueObjectName = "Spouse", Name = "Wife" });
            ent.MemberCollection.Add(new ValueObjectMember { ValueObjectName = "Child", Name = "Children", AllowMultiple = true});



            var contacts = new SimpleMember { Name = "ContactCollection", Type = typeof(Array) };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ent.MemberCollection.Add(contacts);
            
            var result = await ent.CompileWithCsharpAsync();

            Assert.True(result.Result, result.ToString());

        }


    }
}