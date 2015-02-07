using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using Newtonsoft.Json;
using NUnit.Framework;

namespace domain.test.entities
{
    [TestFixture]
    public class EntityDefinitionCodeTest
    {
        [Test]
        public void GenerateRootWithDefaultValues()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());

            var ent = new EntityDefinition { Name = "Lead", Id = "lead", Plural = "Leads", RecordName = "Name" };
            ent.MemberCollection.Add(new Member
            {
                Name = "Name",
                Type = typeof(string),
                IsFilterable = true,
                DefaultValue = new ConstantField { Value = "<Name>", Type = typeof(string) }
            });
            ent.MemberCollection.Add(new Member
            {
                Name = "FullName",
                Type = typeof(string),
                IsFilterable = true,
                DefaultValue = new DocumentField { Path = "Name", Type = typeof(string) }
            });

            ent.MemberCollection.Add(new Member
            {
                Name = "LastName",
                Type = typeof(string),
                IsFilterable = true,
                DefaultValue = new FunctionField { Script = "item.Name.Split(new string[]{\" \"}, StringSplitOptions.RemoveEmptyEntries)[0]" }
            });

            ent.MemberCollection.Add(new Member
            {
                Name = "Title",
                Type = typeof(string),
                IsFilterable = true
            });
            ent.MemberCollection.Add(new Member
            {
                Name = "Rating",
                Type = typeof(int),
                IsFilterable = true,
                DefaultValue = new ConstantField { Value = 1, Type = typeof(int) }
            });
            ent.MemberCollection.Add(new Member
            {
                Name = "RegisteredDate",
                Type = typeof(DateTime),
                IsFilterable = true,
                DefaultValue = new FunctionField { Script = "new DateTime(2011,5,2)", ScriptEngine = new RoslynScriptEngine() }
            });
            var address = new Member { Name = "Address", Type = typeof(object) };
            var malaysia = new ConstantField { Type = typeof(string), Value = "Malaysia" };
            address.MemberCollection.Add(new Member { Name = "Street1", IsFilterable = false, Type = typeof(string) });
            address.MemberCollection.Add(new Member { Name = "State", IsFilterable = true, Type = typeof(string) });
            address.MemberCollection.Add(new Member { Name = "Country", IsFilterable = true, Type = typeof(string), DefaultValue = malaysia });
            ent.MemberCollection.Add(address);
            var options = new CompilerOptions
            {
                IsVerbose = false,
                IsDebug = true
            };

            var contacts = new Member { Name = "ContactCollection", AllowMultiple = true };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ent.MemberCollection.Add(contacts);

            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\System.Web.Mvc.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\core.sph\bin\core.sph.dll"));
            options.ReferencedAssembliesLocation.Add(Path.GetFullPath(@"\project\work\sph\source\web\web.sph\bin\Newtonsoft.Json.dll"));


            byte[] buffers;
            using (var stream = new MemoryStream())
            {
                options.Stream = stream;
                options.Emit = true;
                var result = ent.Compile(options);
                PrintErrors(result, ent);

                Assert.IsTrue(result.Result, result.ToJsonString(Formatting.Indented));
                buffers = stream.GetBuffer();
            }


            var assembly = Assembly.Load(buffers);
            var type = assembly.GetType("Bespoke.Dev_lead.Domain.Lead");
            Assert.IsNotNull(type, type.FullName + " is null");

            dynamic lead = Activator.CreateInstance(type);
            Assert.AreEqual(1, lead.Rating);
            Assert.AreEqual(DateTime.Parse("2011-05-02"), lead.RegisteredDate);


        }

        public static void PrintErrors(SphCompilerResult result, EntityDefinition ent)
        {
            if (result.Result) return;
            Console.WriteLine("!!!!! Failed to build ");
            var codes = ent.GenerateCode();
            var sources = ent.SaveSources(codes);
            sources.ToList().ForEach(x => Console.WriteLine("saved to : {0}", x));
            result.Errors.ForEach(Console.WriteLine);
        }


        [Test]
        public void GenerateCodeBasic()
        {
            var ent = new EntityDefinition { Name = "Customer", Plural = "Customers", RecordName = "Name2" };
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

            var contacts = new Member { Name = "ContactCollection", AllowMultiple = true };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ent.MemberCollection.Add(contacts);



            var result = ent.Compile(options);
            PrintErrors(result, ent);

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


            var contacts = new Member { Name = "ContactCollection", AllowMultiple = true };
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
