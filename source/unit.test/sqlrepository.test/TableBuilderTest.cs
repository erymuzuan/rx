using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;
using Bespoke.Sph.SqlRepository.Compilers;
using Bespoke.Sph.SqlRepository.Extensions;
using Bespoke.Sph.Tests.SqlServer.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.SqlServer
{
    public class TableBuilderTest
    {
        public ITestOutputHelper Console { get; }

        public TableBuilderTest(ITestOutputHelper console)
        {
            Console = console;
            ObjectBuilder.AddCacheList<ISqlServerMetadata>(new MockSqlServerMetadata());
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
        }


        [Fact]
        public void GenerateColumn()
        {
            var ent = new EntityDefinition { Name = "CustomerAccount", Plural = "Customeraccounts", Id = "customer-account", RecordName = "Name" };
            ent.AddMember("Name", typeof(string), true);
            ent.AddMember("No", typeof(string), true);
            ent.AddMember("RegistratioNo", typeof(string), true);
            ent.AddMember("CreditLimit", typeof(decimal), true);

            var address = new ComplexMember { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.AddMember("Street1", typeof(string), false);
            address.AddMember("Postcode", typeof(string), true);
            address.AddMember("State", typeof(string), true);
            ent.MemberCollection.Add(address);


            var columns = ent.GetFilterableMembers().ToList();

            Assert.All(columns, Assert.NotNull);
            Assert.Contains(columns, c => c.FullName == "Address.State");
        }

        [Fact]
        public async Task GenerateCreateTableSql()
        {
            var ed = new EntityDefinition { Name = "CustomerAccount", Plural = "CustomerAccounts", Id = "customer-account", RecordName = "Name" };
            ed.AddMember("Name", typeof(string), true);
            ed.AddMember("No", typeof(string), true);
            ed.AddMember("RegistratioNo", typeof(string), true);
            ed.AddMember("CreditLimit", typeof(decimal), true);

            var address = new ComplexMember { Name = "Address", TypeName = "System.Object, mscorlib" };
            address.AddMember("Street1", typeof(string), false);
            address.AddMember("Postcode", typeof(string), true);
            address.AddMember("State", typeof(string), true);
            ed.MemberCollection.Add(address);


            var builder = new SqlTableBuilder(m => Console.WriteLine("Info :" + m), m => Console.WriteLine("Warning :" + m),
                e => Console.WriteError(e));


            var sources = (await builder.GenerateCodeAsync(ed)).ToList();
            Console.WriteLine(sources.ToString(",", x => x.Name));
            var sql = sources.Single(x => x.Name =="CustomerAccount.sql").GetCode();
            Assert.Contains("[Address.State]", sql);

            Assert.Single(sources.Where(x => x.Name == "CustomerAccount.Index.Name.sql"));

        }

        [Fact]
        public async Task AttachPropertyColumnLength()
        {
            var ed = new EntityDefinition { Name = "CustomerAccount", Plural = "CustomerAccounts", Id = "customer-account", RecordName = "Name" };
            var name = ed.AddMember("Name", typeof(string), true);
            var properties = new[] { new AttachedProperty("Length", 500) };

            var builder = new SqlTableBuilder(m => Console.WriteLine("Info :" + m), m => Console.WriteLine("Warning :" + m),
                e => Console.WriteError(e));
            await builder.SaveAttachPropertiesAsycn(name, properties);


            var sources = (await builder.GenerateCodeAsync(ed)).ToList();
            Console.WriteLine(sources.ToString(",", x => x.Name));
            var sql = sources.Single(x => x.Name =="CustomerAccount.sql").GetCode();
            Assert.Contains("[Name] AS CAST(JSON_VALUE([Json], '$.Name') AS VARCHAR(500))", sql);
        }

        [Fact]
        public void AttachPropertyOtherFieldsIndexMember()
        {
            var ed = new EntityDefinition { Name = "CustomerAccount", Plural = "CustomerAccounts", Id = "customer-account", RecordName = "Name" };
            var name = ed.AddMember("Name", typeof(string), true);

            var properties = new[]
            {
                new AttachedProperty("Indexed", true),
                new AttachedProperty("IndexedFields", "No,CreditLimit")
            };

            var sql = name.CreateIndex(ed, properties);
            Assert.Contains("[No],", sql);
        }
    }
}
