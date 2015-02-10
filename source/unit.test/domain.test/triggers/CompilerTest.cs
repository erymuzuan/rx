using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.QueryProviders;
using domain.test.reports;
using NUnit.Framework;

namespace domain.test.triggers
{
    [TestFixture]
    public class CompilerTest
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
        public async Task Compile()
        {
            var ed = this.CreateAccountDefinition();
            var account = this.CreateInstance(ed);
            Assert.IsNotNull(account);

            m_efMock.AddToDictionary("System.Linq.IQueryable`1[Bespoke.Sph.Domain.EntityDefinition]", ed.Clone());


            var trigger = new Trigger
            {
                Entity = ed.Name,
                IsFiredOnAdded = true,
                IsFiredOnChanged = true,
                IsFiredOnDeleted = true,
                FiredOnOperations = "Save,Publish",
                Id = 33.ToString(CultureInfo.CurrentCulture)
            };
            var options = new CompilerOptions
            {
                IsVerbose = true,
                IsDebug = true
            };
            var result = await trigger.CompileAsync(options);
            result.Errors.ForEach(Console.WriteLine);
            Assert.IsTrue(result.Result);
        }



        private async Task<dynamic> CreateInstance(EntityDefinition ed, bool verbose = false)
        {
            using (var stream = new MemoryStream())
            {
                var options = new CompilerOptions
                {
                    IsVerbose = verbose,
                    IsDebug = true,
                    Emit = true,
                    Stream = stream
                };
                var result = await ed.CompileAsync(options);
                result.Errors.ForEach(Console.WriteLine);

                // try to instantiate the EntityDefinition
                var assembly = Assembly.Load(stream.GetBuffer());
                var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed.Id, ed.Name);

                var edType = assembly.GetType(edTypeName);
                Assert.IsNotNull(edType, edTypeName + " is null in ");

                return Activator.CreateInstance(edType);
            }
        }


        public EntityDefinition CreateAccountDefinition()
        {
            var ent = new EntityDefinition
            {
                Name = "Account",
                Plural = "Accounts",
                RecordName = "AccountNo",
                Id = "72"
            };
            ent.MemberCollection.Add(new Member
            {
                Name = "AccountNo",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            }); ent.MemberCollection.Add(new Member
            {
                Name = "Name",
                TypeName = "System.String, mscorlib",
                IsFilterable = true
            });
            var address = new Member { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.MemberCollection.Add(new Member { Name = "Street1", IsFilterable = false, TypeName = "System.String, mscorlib" });
            address.MemberCollection.Add(new Member { Name = "State", IsFilterable = true, TypeName = "System.String, mscorlib" });
            ent.MemberCollection.Add(address);


            var contacts = new Member { Name = "ContactCollection", Type = typeof(Array) };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ent.MemberCollection.Add(contacts);

            return ent;

        }
    }
}
