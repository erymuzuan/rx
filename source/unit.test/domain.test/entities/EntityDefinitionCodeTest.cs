﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RoslynScriptEngines;
using Xunit;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Extensions;
using domain.test.Extensions;

namespace domain.test.entities
{

    public class EntityDefinitionCodeTest
    {
        [Fact]
        public async Task GenerateRootWithDefaultValues()
        {
            ObjectBuilder.AddCacheList<IScriptEngine>(new RoslynScriptEngine());

            var ent = new EntityDefinition { Name = "SalesLead", Id = "sales-lead", Plural = "Leads", RecordName = "Name" };
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Name",
                Type = typeof(string),
                DefaultValue = new ConstantField { Value = "<Name>", Type = typeof(string) }
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Title",
                Type = typeof(string),
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "IsApproved",
                Type = typeof(bool),
                DefaultValue = new ConstantField { Type = typeof(bool), Name = "True", Value = "true" }
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Rating",
                Type = typeof(int),
                DefaultValue = new ConstantField { Value = 1, Type = typeof(int) }
            });
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "RegisteredDate",
                Type = typeof(DateTime),
                DefaultValue = new FunctionField { Script = "new DateTime(2011,5,2)", ScriptEngine = new RoslynScriptEngine() }
            });
            var address = new ComplexMember { Name = "Address", TypeName = "Alamat" };
            address.MemberCollection.Add(new SimpleMember { Name = "Street1", Type = typeof(string) });
            address.MemberCollection.Add(new SimpleMember { Name = "State", Type = typeof(string) });
            ent.MemberCollection.Add(address);



            var contacts = new ComplexMember { Name = "Contacts", AllowMultiple = true, TypeName = "Contact" };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ent.MemberCollection.Add(contacts);



            var result = await ent.CompileWithCsharpAsync();


            Assert.True(result.Result, result.ToString());

            var dll = AppDomain.CurrentDomain.BaseDirectory + "\\" + Path.GetFileName(result.Output);
            File.Copy(result.Output, dll, true);

            var assembly = Assembly.LoadFrom(dll);
            var type = assembly.GetType($"{ent.CodeNamespace}.{ent.Name}");
            Assert.NotNull(type);

            dynamic lead = Activator.CreateInstance(type);
            Assert.Equal(1, lead.Rating);
            Assert.Equal(DateTime.Parse("2011-05-02"), lead.RegisteredDate);


        }




        [Fact]
        public void GetMembersPath()
        {
            var ent = new EntityDefinition { Name = "Customer", Plural = "Customers" };
            ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Name2",
                TypeName = "System.String, mscorlib",
            }); ent.MemberCollection.Add(new SimpleMember
            {
                Name = "Title",
                TypeName = "System.String, mscorlib",
            });
            var address = new ComplexMember { Name = "Address" };
            address.MemberCollection.Add(new SimpleMember { Name = "Street1", TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new SimpleMember { Name = "State", TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);


            var contacts = new ComplexMember { Name = "Contacts", AllowMultiple = true };
            contacts.MemberCollection.Add(new SimpleMember { Name = "Name", Type = typeof(string) });
            contacts.MemberCollection.Add(new SimpleMember { Name = "Telephone", Type = typeof(string) });
            contacts.MemberCollection.Add(address);
            ent.MemberCollection.Add(contacts);



            var paths = ent.GetMembersPath();
            Assert.Contains("Name2", paths);
            Assert.Contains("Address.State", paths);
            Assert.Contains("Contacts.Name", paths);
            Assert.Contains("Contacts.Address.State", paths);


        }
    }
}
